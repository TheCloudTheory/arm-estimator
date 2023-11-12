param name string
param rgLocation string = resourceGroup().location
param capacity int
param skuName string

resource redis 'Microsoft.Cache/redisEnterprise@2022-01-01' = {
  name: name
  location: rgLocation
  sku: {
    capacity: capacity
    name: skuName
  }
  properties: {
    minimumTlsVersion: '1.2'
  }
}
