using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Cors;
using Profile;

namespace dummy_gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(option => option.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddControllers();
            services.AddLogging(builder => builder.AddConsole());
            services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
            {
                options.Address = new Uri(Configuration.GetValue<string>("ProfileServiceAddress"));
            });
            services.AddGrpcClient<ScoreService.ScoreServiceClient>(options =>
            {
                options.Address = new Uri(Configuration.GetValue<string>("ScoreServiceAddress"));
            });
            services.AddHttpClient("ScoreServiceHttp", client => {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("ScoreServiceAddressHttp"));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "dummy_gateway", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dummy_gateway v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
