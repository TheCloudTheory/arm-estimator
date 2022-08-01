# ARM Cost Estimator
Automated cost estimation of your Azure infrastructure made easy. Works with ARM Templates and Bicep.

## Demo
![arm-estimator-demo](docs/arm-estimator.gif)

## Philosophy
As adoption of cloud services progresses, understanding how cloud billing works becomes more and more critical for keeping everything under control. Most of the time initial infrastructure cost estimation is done only during design phase and gets neglected as development progresses. Many development teams don't have enough understanding how to calculate impact of their changes and find difficult to get an immediate feedback whether they're still withing acceptable level of money spent for their services.

Infrastructure-as-Code (IaC) makes things even more difficult - it solves the problem of cloud infrastructure treated as a separate development stream, but doesn't give you control over cost of components under your control.

ARM Cost Estimator follows a concept of [_running cost as architecture fitness function_](https://www.thoughtworks.com/radar/techniques/run-cost-as-architecture-fitness-function). You can make it an integral part of your CICD pipeline and quickly gather information of how much you're going to spend.

## Installation
ARM Cost Estimator can be download as ZIP package containing a single executable file. Check releases to find the most recent version download URL.

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
dotnet arm-estimator <template-path>.json <subscription-id> <resource-group>
```

## Main features
* Detailed output containing information about cost of your infrastructure and metrics used for calculation
* Seamless integration with ARM Templates and Bicep (with a little help of Bicep CLI)
* Always fresh data thanks to direct calls to Azure Retail API
* Native tool experience - no third-party services / proxies, everything relies on componenets delivered and used by Microsoft
* Multi-option authentication based on `Azure.Identity` package - project automatically uses cached credentials from the running environment (supports Azure CLI / Environment credentials / Managed Identity and more)
* Allows you to validate your deployment before it happens - if the template you used is invalid, an error with detailed information is returned
* Support for both Incremental / Complete deployment modes (see below for details)
* Displaying delta describing difference between your current estimated cost and after changes are applied
* An option to stop CICD process if estimations exceeds given limit (see below for details)

## Known limitations
ARM Cost Estimator is currently in `alpha` development phase meaning there're no guarantees for stable interface and many features are still in design or planning phase. The main limitations as for now are:
* You can use the project only with a resource group as deployment scope
* Some services are in TBD state (see below for more information)
* You cannot generate an output as artifact
* Retail API responses are not cached meaning sometimes getting an output takes more than normally
* There's no possibility to define custom usage patterns so some metrics (mainly those described as price per second / hour / day) are projected for full month
* Nested resources are not supported yet - however, you can define them as separated entities to mitigate that issue

Those limitations will be removed in the future releases of the project.

## Services support
Services not listed are considered TBD.
Service|Support level|More information
----|----|----
Advanced Data Security|Not Supported|-
Advanced Threat Protection|Not Supported|-
AKS|Partial|Estimates work for managed service (both Free / Paid), estimation doesn't include agent pools
APIM|Full|-
App Configuration|Full|-
Application Gateway|Full|-
Application Insights|Partial|Supports classic mode, doesn't support Enteprise Nodes and Multi-step Web Test
Active Directory B2C|Not Supported|-
Active Directory Domain Services|Not Supported|-
Analysis Services|Full|-
Azure App Service|Partial|Supports Azure App Service Plans (without Isolated tiers) and Azure Functions (Consumption / Premium / App Service Plan)
Bastion|Full|-
Bot Service|Full|-
Chaos Studio|Full|-
Cognitive Search|Partial|Doesn't support Document Cracking / Semantic Search / Custom Entity Skills Text Records
Confidential Ledger|Full|Official pricing will be available September 2022
Container Apps|Full|-
Container Registry|Full|-
Cosmos DB|Partial|Supports only single-region writes with manual throughput provisioning
Health Bot|Full|-
SQL Database|Partial|Supports only Databases (DTU model - Basic & Standard)
Storage Account|Partial|Supports only StorageV2 (without File Service)

## Deployment mode
When performing resource group level deployment there's an option to select a deployment mode. ARM Cost Estimator also supports that option by providing desired value as parameter:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --mode Incremental|Complete
```

When parameter is not passed, `Incremental` mode is selected. Selecting `Complete` changes the way estimations work - if there're existing resources in a resource group, they will be considered as up for removal. It'll be noted by ARM Cost Estimator and deducted from the final estimation.

## Threshold
With ARM Cost Estimator it's possible to stop your CICD process is projected estimation exceeds your assumptions:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --threshold <int>
```

By using `--threshold` option, you can set an upper limit for infrastructure cost and make sure, that you can re-evaluate changes before they reach cloud environment and affect your billing.
## Contributions
Contributions are more than welcome!
