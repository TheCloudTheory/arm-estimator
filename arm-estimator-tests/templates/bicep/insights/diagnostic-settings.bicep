param dsName string
param dsName2 string
param dsName3 string
param workspaceId string
param storageAccountId string
param serverName string
param dbName string
param eventHubName string
param location string = resourceGroup().location

resource server 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: 'ace'
    administratorLoginPassword: '123456678'
  }
}

resource db 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: server
  name: dbName
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
  }
}

resource diagnosticSettingsLA 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  scope: db
  name: dsName
  properties: {
    workspaceId: workspaceId
    logs: [
      {
        category: 'SQLSecurityAuditEvents'
        enabled: true
        retentionPolicy: {
          days: 0
          enabled: false
        }
      }
      {
        category: 'DevOpsOperationsAudit'
        enabled: true
        retentionPolicy: {
          days: 0
          enabled: false
        }
      }
    ]
  }
}

resource diagnosticSettingsStorage 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  scope: db
  name: dsName2
  properties: {
    storageAccountId: storageAccountId
    logs: [
      {
        category: 'SQLSecurityAuditEvents'
        enabled: true
        retentionPolicy: {
          days: 0
          enabled: false
        }
      }
      {
        category: 'DevOpsOperationsAudit'
        enabled: true
        retentionPolicy: {
          days: 0
          enabled: false
        }
      }
    ]
  }
}

resource diagnosticSettingsEventHub 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  scope: db
  name: dsName3
  properties: {
    eventHubName: eventHubName
    logs: [
      {
        category: 'SQLSecurityAuditEvents'
        enabled: true
        retentionPolicy: {
          days: 0
          enabled: false
        }
      }
      {
        category: 'DevOpsOperationsAudit'
        enabled: true
        retentionPolicy: {
          days: 0
          enabled: false
        }
      }
    ]
  }
}
