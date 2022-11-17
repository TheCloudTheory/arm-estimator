resource db1 'Microsoft.DBforPostgreSQL/servers@2017-12-01' = {
  name: 'db1'
  location: resourceGroup().location
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
      geoRedundantBackup: 'Disabled'
    }
  }
}

resource db2 'Microsoft.DBforPostgreSQL/servers@2017-12-01' = {
  name: 'db2'
  location: resourceGroup().location
  sku: {
    name: 'GP_Gen5_4'
    tier: 'GeneralPurpose'
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
      geoRedundantBackup: 'Disabled'
    }
  }
}

resource db3 'Microsoft.DBforPostgreSQL/servers@2017-12-01' = {
  name: 'db3'
  location: resourceGroup().location
  sku: {
    name: 'GP_Gen5_8'
    tier: 'GeneralPurpose'
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
      geoRedundantBackup: 'Disabled'
    }
  }
}

resource db4 'Microsoft.DBforPostgreSQL/servers@2017-12-01' = {
  name: 'db4'
  location: resourceGroup().location
  sku: {
    name: 'MO_Gen5_8'
    tier: 'MemoryOptimized'
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
      geoRedundantBackup: 'Disabled'
    }
  }
}

resource db5 'Microsoft.DBforPostgreSQL/flexibleServers@2021-06-01' = {
  name: 'db5'
  location: resourceGroup().location
  sku: {
    name: 'Standard_B1MS_1'
    tier: 'Burstable'
  }
  properties: {
    administratorLogin: 'login'
    administratorLoginPassword: 'password'
    storage: {
      storageSizeGB: 5
    }
  }
}

resource db6 'Microsoft.DBforPostgreSQL/flexibleServers@2021-06-01' = {
  name: 'db6'
  location: resourceGroup().location
  sku: {
    name: 'Standard_B2S_2'
    tier: 'Burstable'
  }
  properties: {
    administratorLogin: 'login'
    administratorLoginPassword: 'password'
    storage: {
      storageSizeGB: 5
    }
  }
}

resource db7 'Microsoft.DBforPostgreSQL/flexibleServers@2021-06-01' = {
  name: 'db7'
  location: resourceGroup().location
  sku: {
    name: 'Standard_Dsv3_2'
    tier: 'GeneralPurpose'
  }
  properties: {
    administratorLogin: 'login'
    administratorLoginPassword: 'password'
    storage: {
      storageSizeGB: 5
    }
  }
}

resource db8 'Microsoft.DBforPostgreSQL/flexibleServers@2021-06-01' = {
  name: 'db8'
  location: resourceGroup().location
  sku: {
    name: 'Standard_Dsv3_16'
    tier: 'GeneralPurpose'
  }
  properties: {
    administratorLogin: 'login'
    administratorLoginPassword: 'password'
    storage: {
      storageSizeGB: 5
    }
  }
}

resource db9 'Microsoft.DBforPostgreSQL/flexibleServers@2021-06-01' = {
  name: 'db9'
  location: resourceGroup().location
  sku: {
    name: 'Standard_Ddsv4_2'
    tier: 'GeneralPurpose'
  }
  properties: {
    administratorLogin: 'login'
    administratorLoginPassword: 'password'
    storage: {
      storageSizeGB: 5
    }
  }
}

resource db10 'Microsoft.DBforPostgreSQL/flexibleServers@2021-06-01' = {
  name: 'db10'
  location: resourceGroup().location
  sku: {
    name: 'Standard_Ddsv4_16'
    tier: 'GeneralPurpose'
  }
  properties: {
    administratorLogin: 'login'
    administratorLoginPassword: 'password'
    storage: {
      storageSizeGB: 5
    }
  }
}
