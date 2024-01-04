param location string = 'westeurope'
param env string = 'dev'
param name string = 'ace'

resource kv 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: 'kv-${name}-${env}-001'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenant().tenantId
    enableRbacAuthorization: true
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'None'
    }
  }
}
