resource existWinVm 'Microsoft.Compute/virtualMachines@2022-03-01' existing = {
  name: 'ace-vm-01'
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: 'ace-la-v1'
  location: 'westeurope'
  properties: any({
    retentionInDays: 30
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  })
}

resource solutionsVMInsights 'Microsoft.OperationsManagement/solutions@2015-11-01-preview' = {
  name: 'VMInsights(ace-la-v1)'
  location: 'westeurope'
  properties: {
    workspaceResourceId: logAnalyticsWorkspace.id
  }
  plan: {
    name: 'VMInsights(ace-la-v1)'
    publisher: 'Microsoft'
    product: 'OMSGallery/VMInsights'
    promotionCode: ''
  }
}

resource virtualMachineName_daExtensionName 'Microsoft.Compute/virtualMachines/extensions@2019-12-01' = {
  parent: existWinVm
  name: 'DependencyAgentWindows'
  location: 'westeurope'
  properties: {
    publisher: 'Microsoft.Azure.Monitoring.DependencyAgent'
    type: 'DependencyAgentWindows'
    typeHandlerVersion: '9.5'
    autoUpgradeMinorVersion: true
  }
}

var WorkspaceResourceId = logAnalyticsWorkspace.id

resource virtualMachineName_mmaExtensionName 'Microsoft.Compute/virtualMachines/extensions@2017-12-01' = {
  parent: existWinVm
  name: 'MMAExtension'
  location: 'westeurope'
  properties: {
    publisher: 'Microsoft.EnterpriseCloud.Monitoring'
    type: 'MicrosoftMonitoringAgent'
    typeHandlerVersion: '1.0'
    autoUpgradeMinorVersion: true
    settings: {
      workspaceId: reference(WorkspaceResourceId, '2015-03-20').customerId
      stopOnMultipleConnections: true
    }
    protectedSettings: {
      workspaceKey: listKeys(WorkspaceResourceId, '2015-03-20').primarySharedKey
    }
  }
}
