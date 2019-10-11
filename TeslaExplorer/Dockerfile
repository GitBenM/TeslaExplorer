FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["TeslaExplorer/TeslaExplorer.csproj", "TeslaExplorer/"]
COPY ["TeslaExplorer.Api/TeslaExplorer.Api.csproj", "TeslaExplorer.Api/"]
COPY ["WebCommon/WebCommon.csproj", "WebCommon/"]
RUN dotnet restore "TeslaExplorer/TeslaExplorer.csproj"
COPY . .
WORKDIR "/src/TeslaExplorer"
RUN dotnet build "TeslaExplorer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TeslaExplorer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TeslaExplorer.dll"]