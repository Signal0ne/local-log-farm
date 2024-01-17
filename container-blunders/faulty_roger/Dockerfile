FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY ./*.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet build --configuration Release --no-restore
CMD ["dotnet", "run"]
