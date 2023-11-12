param rgLocation string = resourceGroup().location

resource la 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'infer-la-cost'
  location: rgLocation
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    workspaceCapping: {
      dailyQuotaGb: 5
    }
  }
}

resource la2 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'infer-la-cost-no-capping'
  location: rgLocation
  properties: {
    sku: {
      name: 'PerGB2018'
    }
  }
}
