FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy AS publish
WORKDIR /src
COPY ./dummy-gateway ./dummy-gateway
COPY ./dummy-proto ./dummy-proto
##OTEL DOTNET AUTOINSTRUMENTATION
ARG OTEL_VERSION=0.7.0
ADD https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v${OTEL_VERSION}/otel-dotnet-auto-install.sh otel-dotnet-auto-install.sh
RUN apt-get update && apt-get install -y unzip && \
    OTEL_DOTNET_AUTO_HOME="/otel-dotnet-auto" sh otel-dotnet-auto-install.sh
##
RUN dotnet restore "./dummy-gateway/dummy-gateway.csproj" && \
    dotnet publish "./dummy-gateway/dummy-gateway.csproj" -c Release -o /app/publish

FROM base as release
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /otel-dotnet-auto /otel-dotnet-auto
ENTRYPOINT ["./dummy-gateway"]