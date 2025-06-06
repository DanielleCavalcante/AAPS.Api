# Estágio base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copia o arquivo .csproj e restaura dependências
COPY ["AAPS.Api/AAPS.Api.csproj", "AAPS.Api/"]
RUN dotnet restore "AAPS.Api/AAPS.Api.csproj"

# Copia o restante do código e builda
COPY . .
WORKDIR "/src/AAPS.Api"
RUN dotnet build "AAPS.Api.csproj" -c Release -o /app/build

# Estágio de publicação
FROM build AS publish
RUN dotnet publish "AAPS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AAPS.Api.dll"]