resource sentinelWorkspace 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: 'ace-sentinel-wksp'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: '90'
  }
}

resource sentinelSolution 'Microsoft.OperationsManagement/solutions@2015-11-01-preview' = {
  name: 'SecurityInsights(ace-sentinel-wksp)'
  location: 'westeurope'
  properties: {
    workspaceResourceId: sentinelWorkspace.id
  }
  plan: {
    name: 'SecurityInsights(ace-sentinel-wksp)'
    publisher: 'Microsoft'
    product: 'OMSGallery/SecurityInsights'
    promotionCode: ''
  }
}
