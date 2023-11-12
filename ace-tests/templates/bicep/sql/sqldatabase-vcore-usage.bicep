param serverName string
param dbName string
param rgLocation string = resourceGroup().location

metadata aceUsagePatterns = {
  Microsoft_Sql_servers_databases_vCore_Storage: '100'
}

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
    name: 'GP_Gen5_2'
  }
  properties: {
  }
}
