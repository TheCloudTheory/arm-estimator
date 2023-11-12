param serverName string
param dbName string
param elasticPoolSku string
param elasticPoolCapacity int
param rgLocation string = resourceGroup().location

resource server 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: serverName
  location: rgLocation
  properties: {
    administratorLogin: 'ace'
    administratorLoginPassword: '123456678'
  }
}

resource pool 'Microsoft.Sql/servers/elasticPools@2021-11-01' = {
  parent: server
  name: 'default'
  location: rgLocation
  sku: {
    name: elasticPoolSku
    capacity: elasticPoolCapacity
  }
  properties: {
    licenseType: 'LicenseIncluded'
  }
}

resource db 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: server
  name: dbName
  location: rgLocation
  sku: {
    name: 'ElasticPool'
    tier: elasticPoolSku
    capacity: 0
  }
  properties: {
    elasticPoolId: pool.id
  }
}
