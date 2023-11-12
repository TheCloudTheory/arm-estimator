resource workspaceName_resource 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: 'ace-la-01'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'pergb2018'
    }
    retentionInDays: '30'
    features: {
      searchVersion: 1
      legacy: 0
    }
  }
}

resource automationAccountName_resource 'Microsoft.Automation/automationAccounts@2021-06-22' = {
  name: 'ace-automationAccount-01'
  location: 'westeurope'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    sku: {
      name: 'Basic'
    }
  }
  dependsOn: [
    workspaceName_resource
  ]
}

resource automationAccountName_samplePowerShellRunbookName 'Microsoft.Automation/automationAccounts/runbooks@2020-01-13-preview' = {
  parent: automationAccountName_resource
  name: 'ace-rb-001'
  location: 'westeurope'
  properties: {
    runbookType: 'PowerShell'
    logProgress: 'false'
    logVerbose: 'false'
    description: 'samplePowerShellRunbookDescription'
    publishContentLink: {
      uri: uri('https://raw.githubusercontent.com/azureautomation/hello-world-for-azure-automation/master/', 'Write-HelloWorld.ps1')
      version: '1.0.0.0'
    }
  }
}

resource workspaceName_Automation 'Microsoft.OperationalInsights/workspaces/linkedServices@2020-08-01' = {
  parent: workspaceName_resource
  name: 'Automation'
  location: 'westeurope'
  properties: {
    resourceId: automationAccountName_resource.id
  }
}
