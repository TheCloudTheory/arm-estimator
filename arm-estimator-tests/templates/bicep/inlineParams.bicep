param location string = resourceGroup().location
param singleLineObject object
param exampleArray array

@secure()
param adminPassword string
param adminLogin string
param minCapacity int

resource dbserver 'Microsoft.Sql/servers@2021-11-01-preview' = {
  name: 'sqlserver'
  location: location
  properties: {
    administratorLogin: adminLogin
    administratorLoginPassword: adminPassword
  }
}

resource dbbasic 'Microsoft.Sql/servers/databases@2021-11-01-preview' = {
  parent: dbserver
  name: 'ace-db'
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    minCapacity: minCapacity
  }
}
