FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DreamWeddingApi/DreamWeddingApi.csproj", "DreamWeddingApi/"]
RUN dotnet restore "DreamWeddingApi/DreamWeddingApi.csproj"
COPY . .
WORKDIR "/src/DreamWeddingApi"
RUN dotnet build "DreamWeddingApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DreamWeddingApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DreamWeddingApi.dll"]
