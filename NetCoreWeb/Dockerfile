FROM microsoft/aspnetcore:2.0-stretch AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-stretch AS build
WORKDIR /src
COPY ["NetCoreWeb/NetCoreWeb.csproj", "NetCoreWeb/"]
COPY ["NetCorePal.HealthCheck.Redis/NetCorePal.HealthCheck.Redis.csproj", "NetCorePal.HealthCheck.Redis/"]
COPY ["NetCorePal.HealthCheck/NetCorePal.HealthCheck.csproj", "NetCorePal.HealthCheck/"]
COPY ["NetCorePal.HealthCheck.AspNetCore/NetCorePal.HealthCheck.AspNetCore.csproj", "NetCorePal.HealthCheck.AspNetCore/"]
RUN dotnet restore "NetCoreWeb/NetCoreWeb.csproj"
COPY . .
WORKDIR "/src/NetCoreWeb"
RUN dotnet build "NetCoreWeb.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NetCoreWeb.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetCoreWeb.dll"]