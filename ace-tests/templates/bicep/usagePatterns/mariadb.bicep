param location string = resourceGroup().location
param name string
param geoRedundantBackup string

metadata aceUsagePatterns = {
  Microsoft_DBforMariaDB_servers_Backup_Storage: '100'
}

resource mariaDbServer01 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: name
  location: location
  sku: {
    name: 'B_Gen5_1'
    tier: 'Basic'
    capacity: 1
    size: '5120' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: 5120
      backupRetentionDays: 7
      geoRedundantBackup: geoRedundantBackup
    }
  }
}
