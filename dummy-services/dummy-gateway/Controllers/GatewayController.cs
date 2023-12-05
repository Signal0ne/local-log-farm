using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Profile;
using static System.Net.Mime.MediaTypeNames;

namespace dummy_gateway{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("MyPolicy")]
    public class GatewayController : ControllerBase{

        private readonly ProfileService.ProfileServiceClient _profileClient;
        private readonly ScoreService.ScoreServiceClient _scoreClient;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _scoreServiceHttpAddr;

        private readonly ILogger<GatewayController> _logger;

        public GatewayController(
            ProfileService.ProfileServiceClient profileClient,
            ScoreService.ScoreServiceClient scoreClient,
            IHttpClientFactory httpClientFactory,
            ILogger<GatewayController> logger
        ){
            _profileClient = profileClient;
            _scoreClient = scoreClient;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        [Route("/get_profile")]
        public GetProfileResponse GetProfileHandler(){
            var id = Request.Query["id"];
            GetProfileResponse resp = new GetProfileResponse();
            GetProfileRequest request = new GetProfileRequest
            {
                Id = id
            };
            try{
                resp = _profileClient.GetProfile(request);
            } catch (Exception e){
                _logger.LogError("Error while calling GetProfile {}", e.Message);
            }
            return resp;
        }

        [HttpPut]
        [Route("/set_profile")]
        public async Task<SetProfileResponse> SetProfile(){
            var body = Request.Body;
            SetProfileResponse resp = new SetProfileResponse();
            SetProfileRequest requestPayload = new SetProfileRequest();
            char[] buffer = new char[2048];
            try{
                StreamReader reader = new StreamReader(body);
                var charNum = await reader.ReadAsync(buffer);
                char[] trimmedBuffer = new char[charNum];
                Array.Copy(buffer, trimmedBuffer, charNum);
                Console.WriteLine(trimmedBuffer);
                requestPayload = JsonSerializer.Deserialize<SetProfileRequest>(new string(trimmedBuffer));
            } catch (Exception e){
                _logger.LogError("Error while decoding request body {}", e.Message);
                return new SetProfileResponse{};
            }
            try{
                resp = _profileClient.SetProfile(requestPayload);
            }catch (Exception e){
                _logger.LogError("Error while calling SetProfile {}", e.Message);
            }
            return resp;
        }

        [HttpPut]
        [Route("/submit_score")]
        public async Task<ScoreResponse> SubmitScore(){
            var downstream = Request.Query["downstream"];
            ScoreResponse r = new ScoreResponse{};
            var body = Request.Body;
            var requestPayload = await DecodeBody(body);
            try{
                switch (downstream){
                    case "grpc":
                        r =  SubmitScoreDownstreamGrpc(requestPayload);
                        break;
                    case "http1_1":
                        r = await SubmitScoreDownstreamHttp1_1(requestPayload);
                        break;
                    default:
                        r = SubmitScoreDownstreamGrpc(requestPayload);
                        break;
                }
            }
            catch (Exception e){
                _logger.LogError("Error while calling SubmitScore {}", e.Message);
            }
            return r;
        }

        private ScoreResponse SubmitScoreDownstreamGrpc(ScoreRequest requestPayload){
            var resp =  _scoreClient.SubmitScore(requestPayload);
            return resp;
        }
        private async Task<ScoreResponse> SubmitScoreDownstreamHttp1_1(ScoreRequest requestPayload){
            var content = new StringContent(
                JsonSerializer.Serialize(requestPayload),
                Encoding.UTF8,
                Application.Json);
            var httpClient = _httpClientFactory.CreateClient("ScoreServiceHttp");
            var httpResponseMessage = await httpClient.PutAsync("/api/submit_score", content);
            var resp = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ScoreResponse>(resp);
        }
        private async Task<ScoreRequest> DecodeBody(Stream body){
            ScoreRequest requestPayload = new ScoreRequest();
            char[] buffer = new char[2048];
            try{
                StreamReader reader = new StreamReader(body);
                var charNum = await reader.ReadAsync(buffer);
                char[] trimmedBuffer = new char[charNum];
                Array.Copy(buffer, trimmedBuffer, charNum);
                Console.WriteLine(trimmedBuffer);
                requestPayload = JsonSerializer.Deserialize<ScoreRequest>(new string(trimmedBuffer));
            } catch (Exception e){
                _logger.LogError("Error while decoding request body {}", e.Message);
                return new ScoreRequest{};
            }
            return requestPayload;
        }
    }
}