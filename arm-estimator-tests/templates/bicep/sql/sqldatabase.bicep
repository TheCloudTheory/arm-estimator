param serverName string
param dbName string
param dbSku string
param rgLocation string = resourceGroup().location

resource server 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: serverName
  location: rgLocation
  properties: {
    administratorLogin: 'ace'
    administratorLoginPassword: '123456678'
  }
}

resource db 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: server
  name: dbName
  location: rgLocation
  sku: {
    name: dbSku
  }
  properties: {
  }
}
