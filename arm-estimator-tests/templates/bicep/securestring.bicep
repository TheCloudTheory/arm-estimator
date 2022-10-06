param location string = resourceGroup().location

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
  name: 'ace-db'
  location: location
  sku: {
    name: 'Basic'
  }
}
