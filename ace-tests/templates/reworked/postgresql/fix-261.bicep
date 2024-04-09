param parSuffix string = utcNow('yyyyMMddhhmmss')
param parLocation string = resourceGroup().location

resource db1 'Microsoft.DBforPostgreSQL/flexibleServers@2022-12-01' = {
#disable-next-line use-stable-resource-identifiers
  name: 'postgresqlserver1-${parSuffix}'
  location: parLocation
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties: {
    createMode: 'Default'
    administratorLogin: 'login'
    administratorLoginPassword: 'password'
    storage: {
      storageSizeGB: 64
    }
  }
}
