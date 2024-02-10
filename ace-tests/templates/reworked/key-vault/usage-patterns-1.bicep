param parSuffix string = utcNow('yyyyMMddhhmmss')
param parLocation string = resourceGroup().location
param parTenantId string = tenant().tenantId

metadata aceUsagePatterns = {
  Microsoft_KeyVault_vaults_Operations: '100000'
  Microsoft_KeyVault_vaults_Advanced_Key_Operations: '100000'
  Microsoft_KeyVault_vaults_Secret_Renewal: '100'
  Microsoft_KeyVault_vaults_Automated_Key_Rotation: '1000'
  Microsoft_KeyVault_vaults_Certificate_Renewal_Requests: '10'
}

resource kv 'Microsoft.KeyVault/vaults@2023-07-01' = {
#disable-next-line use-stable-resource-identifiers
  name: 'kv${parSuffix}'
  location: parLocation
  properties: {
    sku: {
      family: 'A'
      name:  'standard'
    }
    tenantId: parTenantId
  }
}
