param parSuffix string = utcNow('yyyyMMddhhmmss')
param parLocation string = resourceGroup().location
param parPrefix string

resource automationAccountName_resource 'Microsoft.Automation/automationAccounts@2021-06-22' = {
#disable-next-line use-stable-resource-identifiers decompiler-cleanup
  name: '${parPrefix}-${parSuffix}'
  location: parLocation
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    sku: {
      name: 'Basic'
    }
  }
}
