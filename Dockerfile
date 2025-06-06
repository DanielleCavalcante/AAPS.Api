FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app
COPY ["AAPS.Api/AAPS.Api/AAPS.Api.csproj", "AAPS.Api/"]
RUN dotnet restore "./AAPS.Api/AAPS.Api.csproj"
COPY . .

RUN dotnet build "AAPS.Api/AAPS.Api/AAPS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AAPS.Api/AAPS.Api/AAPS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AAPS.Api.dll"]