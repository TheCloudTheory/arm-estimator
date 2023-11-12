param location string = resourceGroup().location
param singleLineObject object
param exampleArray array
param dbName string
param serverName string

@secure()
param adminPassword string
param adminLogin string
param minCapacity int

resource dbserver 'Microsoft.Sql/servers@2021-11-01-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: adminLogin
    administratorLoginPassword: adminPassword
  }
}

resource dbbasic 'Microsoft.Sql/servers/databases@2021-11-01-preview' = {
  parent: dbserver
  name: dbName
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    minCapacity: minCapacity
  }
}
