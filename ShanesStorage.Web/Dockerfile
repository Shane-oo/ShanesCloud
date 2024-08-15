FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShanesStorage.Web/ShanesStorage.Web.csproj", "ShanesStorage.Web/"]
RUN dotnet restore "ShanesStorage.Web/ShanesStorage.Web.csproj"
COPY . .
WORKDIR "/src/ShanesStorage.Web"
RUN dotnet build "ShanesStorage.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ShanesStorage.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShanesStorage.Web.dll"]
