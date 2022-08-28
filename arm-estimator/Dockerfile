FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["arm-estimator/arm-estimator.csproj", "arm-estimator/"]
RUN dotnet restore "arm-estimator/arm-estimator.csproj"
COPY . .
WORKDIR "/src/arm-estimator"
RUN dotnet build "arm-estimator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "arm-estimator.csproj" -c Release -o /app/publish -p:PublishSingleFile=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y curl
RUN curl -Lo bicep https://github.com/Azure/bicep/releases/latest/download/bicep-linux-x64
RUN chmod +x ./bicep
RUN mv ./bicep /usr/local/bin/bicep
ENTRYPOINT ["dotnet", "arm-estimator.dll"]