// ######################################
// Basic SKU - Azure Database for MySQLDB
// ######################################

resource mysqlDbServer01 'Microsoft.DBforMySQL/servers@2017-12-01' = {
  name: 'ace-mysqldb-01'
  location: 'westeurope'
  sku: {
    name: 'B_Gen5_1'
    tier: 'Basic'
    capacity: 1
    size: '5120'  
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '8.0'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    storageProfile: {
      storageMB: 5120
    }
  }
}

resource mysqlDbServer02 'Microsoft.DBforMySQL/servers@2017-12-01' = {
  name: 'ace-mysqldb-02'
  location: 'westeurope'
  sku: {
    name: 'B_Gen5_2'
    tier: 'Basic'
    capacity: 2
    size: '5120'  
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '8.0'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    storageProfile: {
      storageMB: 5120
    }
  }
}

resource mysqlDbServer03 'Microsoft.DBforMySQL/servers@2017-12-01' = {
  name: 'ace-mysqldb-03'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5_2'
    tier: 'GeneralPurpose'
    capacity: 2
    size: '5120'  
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '8.0'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    storageProfile: {
      storageMB: 5120
    }
  }
}

resource mysqlDbServer04 'Microsoft.DBforMySQL/servers@2017-12-01' = {
  name: 'ace-mysqldb-04'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5_4'
    tier: 'GeneralPurpose'
    capacity: 4
    size: '5120'  
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '8.0'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    storageProfile: {
      storageMB: 5120
    }
  }
}

resource mysqlDbServer05 'Microsoft.DBforMySQL/servers@2017-12-01' = {
  name: 'ace-mysqldb-05'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5_8'
    tier: 'GeneralPurpose'
    capacity: 8
    size: '5120'  
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '8.0'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    storageProfile: {
      storageMB: 5120
    }
  }
}

resource mysqlDbServer06 'Microsoft.DBforMySQL/servers@2017-12-01' = {
  name: 'ace-mysqldb-06'
  location: 'westeurope'
  sku: {
    name: 'MO_Gen5_2'
    tier: 'MemoryOptimized'
    capacity: 2
    size: '5120'  
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '8.0'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    storageProfile: {
      storageMB: 5120
    }
  }
}

resource mysqlDbServer07 'Microsoft.DBforMySQL/servers@2017-12-01' = {
  name: 'ace-mysqldb-07'
  location: 'westeurope'
  sku: {
    name: 'MO_Gen5_4'
    tier: 'MemoryOptimized'
    capacity: 4
    size: '5120'  
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '8.0'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    storageProfile: {
      storageMB: 5120
    }
  }
}

resource mysqlDbServer08 'Microsoft.DBforMySQL/servers@2017-12-01' = {
  name: 'ace-mysqldb-08'
  location: 'westeurope'
  sku: {
    name: 'MO_Gen5_8'
    tier: 'MemoryOptimized'
    capacity: 8
    size: '5120'  
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '8.0'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    storageProfile: {
      storageMB: 5120
    }
  }
}
