param rgLocation string = resourceGroup().location

metadata aceUsagePatterns = {
  Microsoft_OperationalInsights_workspaces_Paug_Data_Ingestion: '50'
}

resource la 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'usagepattern-la-cost'
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
  name: 'usagepattern-la-cost-no-capping'
  location: rgLocation
  properties: {
    sku: {
      name: 'PerGB2018'
    }
  }
}
