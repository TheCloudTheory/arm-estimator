param paramSkuName string = 'F1'

resource appserviceplan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: 'myAppServicePlan'
  location: resourceGroup().location
  sku: {
    name: paramSkuName
  }
  properties: {
    reserved: true
  }
}

resource webapp 'Microsoft.Web/sites@2023-12-01' = {
  name: 'myWebApp'
  location: resourceGroup().location
  properties: {
    serverFarmId: appserviceplan.id
  }
}
