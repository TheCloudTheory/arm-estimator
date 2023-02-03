param location string = resourceGroup().location
param dbName string
param serverName string

@secure()
param adminPassword string

resource dbserver 'Microsoft.Sql/servers@2021-11-01-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: 'adminace'
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
}
