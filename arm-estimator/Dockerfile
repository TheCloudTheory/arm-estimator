FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["arm-estimator/azure-cost-estimator.csproj", "ace/"]
RUN dotnet restore "ace/azure-cost-estimator.csproj"
COPY . .
WORKDIR "/src/ace"
RUN dotnet build "azure-cost-estimator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "azure-cost-estimator.csproj" -c Release -o /app/publish -p:PublishSingleFile=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y curl
RUN curl -Lo bicep https://github.com/Azure/bicep/releases/latest/download/bicep-linux-x64
RUN chmod +x ./bicep
RUN mv ./bicep /usr/local/bin/bicep
ENTRYPOINT ["dotnet", "azure-cost-estimator.dll"]