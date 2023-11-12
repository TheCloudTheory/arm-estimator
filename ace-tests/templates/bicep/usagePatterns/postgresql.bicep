param location string = resourceGroup().location
param name string
param geoRedundantBackup string

metadata aceUsagePatterns = {
  Microsoft_DBforPostgreSQL_servers_Backup_Storage: '100'
}

resource db1 'Microsoft.DBforPostgreSQL/servers@2017-12-01' = {
  name: name
  location: location
  sku: {
    name: 'B_Gen5_2'
    tier: 'Basic'
    capacity: 1
    size: '5120'
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    administratorLogin: 'login'
    administratorLoginPassword: 'password'
    storageProfile: {
      storageMB: 5120
      backupRetentionDays: 7
      geoRedundantBackup: geoRedundantBackup
    }
  }
}
