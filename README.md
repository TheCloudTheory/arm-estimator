# ARM Cost Estimator
Automated cost estimation of your Azure infrastructure made easy.

## Usage
You can use the project with both ARM Templates and (indirectly) with Bicep files. Due to limitation of Azure What If API, your Bicep definitions must be transformed to ARM Templates before you can use them with ARM Cost Estimator. This can be done with a simple command:
```
bicep build <your-bicep-file>.bicep
```
This will create an ARM Template based on the Bicep file passed as argument.
### Windows
```
arm-estimator.exe <template-path>.json <subscription-id> <resource-group>
```
### Linux
```
dotnet arm-estimator.dll <template-path>.json <subscription-id> <resource-group>
```

## Main features
* Detailed output containing information about cost of your infrastructure and metrics used for calculation
* Seamless integration with ARM Templates and Bicep (with a little help of Bicep CLI)
* Always fresh data thanks to direct calls to Azure Retail API
* Native tool experience - no third-party services / proxies, everything relies on componenets delivered and used by Microsoft
* Multi-option authentication based on `Azure.Identity` package - project automatically uses cached credentials from the running environment (supports Azure CLI / Environment credentials / Managed Identity and more)
* Allows you to validate your deployment before it happens - if the template you used is invalid, an error with detailed information is returned

## Known limitations
ARM Cost Estimator is currently in `alpha` phase development meaning there're no guarantees for stable interface and many features are still in development or planning phase. The main limitations as for now are:
* You can use the project only with a resource group as deployment scope
* Some services are in TBD state (see below for more information)
* You cannot generate an output as artifact
* Currently only changes detected as `Create` are supported
* Retail API responses are not cached meaning sometimes getting an output takes more than normally
* There's no possibility to define custom usage patterns so some metrics (mainly those described as price per second / hour / day) are projected for full month

Those limitations will be removed in the future releases of the project.

## Services support
Services not listed are considered TBD.
Service|Support level|More information
----|----|----
AKS|Partial|Estimates work for managed service (both Free / Paid), estimation doesn't include agent pools
Azure App Service|Partial|Supports Azure App Service Plans (without Isolated tiers) and Azure Functions (Consumption / Premium / App Service Plan)
Container Apps|Full|-
Container Registry|Full|-
Azure SQL|Partial|Supports only Databases (DTU model - Basic & Standard)
Storage Account|Partial|Supports only StorageV2 (without File Service)

## Contributions
Contributions are more than welcome!
