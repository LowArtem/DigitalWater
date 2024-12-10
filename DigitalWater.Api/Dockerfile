FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["DigitalWater.Api/DigitalWater.Api.csproj", "DigitalWater.Api/"]
COPY ["DigitalWater.Data/DigitalWater.Data.csproj", "DigitalWater.Data/"]
COPY ["DigitalWater.Core/DigitalWater.Core.csproj", "DigitalWater.Core/"]
RUN dotnet restore "DigitalWater.Api/DigitalWater.Api.csproj"
COPY . .
WORKDIR "/src/DigitalWater.Api"
RUN dotnet build "DigitalWater.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DigitalWater.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigitalWater.Api.dll"]
