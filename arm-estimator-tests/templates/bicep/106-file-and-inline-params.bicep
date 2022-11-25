param dbName string
param location string
param sku string

@secure()
param adminPassword string

resource dbserver 'Microsoft.Sql/servers@2021-11-01-preview' = {
  name: 'sqlserver'
  location: location
  properties: {
    administratorLogin: 'adminace'
    administratorLoginPassword: adminPassword
  }
}

resource dbbasic 'Microsoft.Sql/servers/databases@2021-11-01-preview' = {
  parent: dbserver
  name: dbName
  location: 'westeurope'
  sku: {
    name: sku
  }
}