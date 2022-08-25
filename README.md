# ACE (Azure Cost Estimator)
Automated cost estimation of your Azure infrastructure made easy. Works with ARM Templates and Bicep.

## Demo
![arm-estimator-demo](docs/arm-estimator.gif)

## Philosophy
As adoption of cloud services progresses, understanding how cloud billing works becomes more and more critical for keeping everything under control. Most of the time initial infrastructure cost estimation is done only during design phase and gets neglected as development progresses. Many development teams don't have enough understanding how to calculate impact of their changes and find difficult to get an immediate feedback whether they're still withing acceptable level of money spent for their services.

Infrastructure-as-Code (IaC) makes things even more difficult - it solves the problem of cloud infrastructure treated as a separate development stream, but doesn't give you control over cost of components under your control.

ACE follows a concept of [_running cost as architecture fitness function_](https://www.thoughtworks.com/radar/techniques/run-cost-as-architecture-fitness-function). You can make it an integral part of your CICD pipeline and quickly gather information of how much you're going to spend.

## Main features
* Detailed output containing information about cost of your infrastructure and metrics used for calculation
* Seamless integration with ARM Templates and Bicep (with a little help of Bicep CLI)
* Always fresh data thanks to direct calls to Azure Retail API
* Native tool experience - no third-party services / proxies, everything relies on componenets delivered and used by Microsoft
* Multi-option authentication based on `Azure.Identity` package - project automatically uses cached credentials from the running environment (supports Azure CLI / Environment credentials / Managed Identity and more)
* Allows you to validate your deployment before it happens - if the template you used is invalid, an error with detailed information is returned
* Support for both Incremental / Complete deployment modes (see `Usage` section)
* Displaying delta describing difference between your current estimated cost and after changes are applied
* An option to stop CICD process if estimations exceed given limit (see `Usage` section)
* Supports passing parameters along with your template
* Handles extension resources as long as they're correctly configured (i.e. define `scope` parameter)
* Supports 17 different currencies
* Allows for generating output as an artifact for further processing

## Installation
ACE can be download as ZIP package containing a single executable file. Check releases to find the most recent version download URL.

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

### Parameters
When using ACE, you must use the following three required parameters:
Name|Default value|Example|Description
---|---|---|---
template-file|N/A|`some_path/some_file.json`|Path to the template file (must be in JSON format)
subscription-id|N/A|`11c43ee8-b9d3-4e51-b73f-bd9dda66e29c`|Identifier (GUID) of your subscription
resource-group|N/A|`mygroup-rg`|Name of the resource group

### Options
Options are non-mandatory parameters, which provide extended functionality for the project. Detailed information how they work can be found below
Name|Default value|Example|Description
---|---|---|---
--mode|`--mode Incremental`|`Complete`|Deployment mode used for calculation. Supports `Incremenetal` and `Complete` deployments
--threshold|`-1`|`--threshold 3000`|Max acceptable estimated cost. Exceeding threshold causes a non-zero exit code to be reported
--parameters|`null`|`--parameters some_path/params.parameters.json`|Path to the parameters file (must be in JSON format)
--currency|`USD`|`--currency EUR`|Currency code to use for estimations
--generateJsonOutput|`false`|`--generateJsonOutput`|Generates JSON file containing estimation result
--silent|`false`|`--silent`|Silences logs so no information is returned to console
--stdout|`false`|`--stdout`|Redirects generated output to stdout instead of file

### Deployment mode
##### Available from: alpha2
When performing resource group level deployment there's an option to select a deployment mode. ACE also supports that option by providing desired value as parameter:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --mode Incremental|Complete
```

When parameter is not passed, `Incremental` mode is selected. Selecting `Complete` changes the way estimations work - if there're existing resources in a resource group, they will be considered as up for removal. It'll be noted by ARM Cost Estimator and deducted from the final estimation.

### Threshold
##### Available from: alpha3
With ACE it's possible to stop your CICD process is projected estimation exceeds your assumptions:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --threshold <int>
```

By using `--threshold` option, you can set an upper limit for infrastructure cost and make sure, that you can re-evaluate changes before they reach cloud environment and affect your billing.
> If estimation exceeds configured threshold, ARM Cost Estimator exits with status code 1. Make sure you check against returned status code and handle that scenario properly. 

Configuring threshold is optional - if you omit it, your CICD process will continue ignoring estimation value.

### Parameters
##### Available from: alpha4
Very often templates contain parameters, which have different values depending on the selected environment. Sometimes you just need to pass a value, which is generated outside your template. ARM Cost Estimator supports parameters in the same way as you'd normally develop them and pass for deployment:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --parameters <path-to-your-parameters-file>.json
```
Both ARM Templates and Bicep use parameters defined as JSON files. ACE expects parameters file to be passed without changes, even though Azure What If API expects sending parameters with slightly different schema than parameters file itself.

When using ACE, you parameters file should look like this:
```
{
  "$schema": "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "dbName": {
      "value": "db"
    },
    "location": {
      "value": "westeurope"
    },
    "sku": {
      "value": "Basic"
    }
  }
}
```

Do not transform it to the schema expected by What If API (which expects value of the `parameters` parameter only).

### Currency
##### Available from: beta1
It's possible to use one of the supported currencies to display estimation result in appropriate format:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --currency EUR
```
For now, you can use one of the following currency codes:
Code|Name
---|---
USD|US dollar
AUD|Australian dollar
BRL|Brazilian real
CAD|Canadian dollar
CHF|Swiss franc
CNY|Chinese yuan
DKK|Danish krone
EUR|Euro
GBP|British pound
INR|Indian rupee
JPY|Japanese yen
KRW|Korean won
NOK|Norwegian krone
NZD|New Zealand dollar
RUB|Russian ruble
SEK|Swedish krona
TWD|Taiwan dollar

Support for a given currency depends on capabilities of underlying Azure Retail API. 

### JSON output
##### Available from: beta1
If you want to use estimation for further automation, it's possible to generate a JSON file containing basic information about estimated resources using `--generateJsonOutput` option:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --generateJsonOutput true
```

If that option is set to `true`, once all data is obtained, a JSON file is created:
```
{
    "TotalCost": 119.3248,
    "Delta": 119.3248,
    "Resources": [
        {
            "Id": "/subscriptions/.../Microsoft.Compute/virtualMachines/ace-vm-01",
            "TotalCost": 29.93,
            "Delta": 29.93
        },
        {
            "Id": "/subscriptions/.../Microsoft.Compute/virtualMachines/ace-vm-02",
            "TotalCost": 63.51,
            "Delta": 63.51
        },
        {
            ...
        }
    ],
    "Currency": "USD"
}
```

Name of the file contains a UTC timestamp - `ace_estimation_yyyyMMddHHssmm.json` - but the file is overwritten if the same timestamp would be used twice.

### Silent mode
##### Available from: beta2
If you don't want any output to be visible in console, you can use `--silent` option for enabling silent mode:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --silent
```

Special use case of that option is using it with output redirection:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --generateJsonOutput --stdout --silent
```

As output redirected to stdout is always considered as non-silent, you can get e.g. estimation JSON without all the noise coming from the tool.

### Output redirection
##### Available from: beta2
If you don't want an output file to be generated, you can use `--stdout` option to redirect generated output to stdout:
```
arm-estimator <template-path>.json <subscription-id> <resource-group> --generateJsonOutput --stdout 
```

This is especially useful when using estimation output as input for another command or process.

## Known limitations
ACE is currently in `beta` development phase meaning there're no guarantees for stable interface and some features are still in design or planning phase. The main limitations as for now are:
* You can use the project only with a resource group as deployment scope
* Some services are in TBD state (see below for more information)
* There's no possibility to define custom usage patterns so some metrics (mainly those described as price per second / hour / day) are projected for full month
* Nested resources are not supported yet - however, you can define them as separated entities to mitigate that issue

Those limitations will be removed in the future releases of the project.

## Services support
Services not listed are considered TBD.
Service|Support level|More information
----|----|----
Advanced Data Security|Not Supported|-
Advanced Threat Protection|Not Supported|-
AKS|In development|Estimates work for managed service (both Free / Paid), estimation doesn't include agent pools
APIM|Stable|-
App Configuration|Stable|-
Application Gateway|Stable|-
Application Insights|In development|Supports classic mode, doesn't support Enteprise Nodes and Multi-step Web Test
Active Directory B2C|Not Supported|-
Active Directory Domain Services|Not Supported|-
Analysis Services|Stable|-
Azure App Service|In development|Supports Azure App Service Plans (without Isolated tiers) and Azure Functions (Consumption / Premium / App Service Plan)
Backup|In Progress|Supports SQL Server in Azure VM, Azure VM, Azure Files, SAP HANA on Azure VM. Supports GRS backup replication only
Bastion|Stable|-
Bot Service|Stable|-
Chaos Studio|Stable|-
Cognitive Search|In development|Doesn't support Document Cracking / Semantic Search / Custom Entity Skills Text Records
Confidential Ledger|Stable|Official pricing will be available September 2022
Container Apps|Stable|-
Container Registry|Stable|-
Cosmos DB|In development|Supports only single-region writes with manual throughput provisioning
Event Hub|Stable|-
Event Grid|Stable|-
Health Bot|Stable|-
Key Vault|Stable|Doesn't support Azure Dedicated HSM
Log Analytics|In development|Estimations doesn't include commitment tiers & logs retention
Logic Apps|In development|Doesn't support ISE scale units
Network Interface|Stable|-
Network Security Group|Stable|-
Public IP Address|Stable|-
Public IP Address Prefixes|Stable|-
Sentinel|In progress|Estimations doesn't include commitment tiers
SignalR|Stable|-
SQL Database|In development|Supports only Databases (DTU model - Basic & Standard)
Storage Account|In development|Supports only StorageV2 (without File Service)
Stream Analytics|Stable|Stream Analytics on Edge requires separate estimation
Time Series|Stable|-
Virtual Machine|In Progress|Support only A, B and D VM family, doesn't support low-priority / spot VMs
Virtual Network|In Progress|Doesn't support VNET peering
VPN Gateway|Stable|-

## Contributions
Contributions are more than welcome!
