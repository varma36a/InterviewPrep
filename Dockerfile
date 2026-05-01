FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SaaSPlatform.sln", "./"]
COPY ["src/SaaSPlatform.Api/SaaSPlatform.Api.csproj", "src/SaaSPlatform.Api/"]
COPY ["src/SaaSPlatform.Application/SaaSPlatform.Application.csproj", "src/SaaSPlatform.Application/"]
COPY ["src/SaaSPlatform.Domain/SaaSPlatform.Domain.csproj", "src/SaaSPlatform.Domain/"]
COPY ["src/SaaSPlatform.Infrastructure/SaaSPlatform.Infrastructure.csproj", "src/SaaSPlatform.Infrastructure/"]
RUN dotnet restore "src/SaaSPlatform.Api/SaaSPlatform.Api.csproj"

COPY . .
RUN dotnet publish "src/SaaSPlatform.Api/SaaSPlatform.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "SaaSPlatform.Api.dll"]
