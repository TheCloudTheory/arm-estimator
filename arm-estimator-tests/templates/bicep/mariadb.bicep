// ######################################
// Basic SKU - Azure Database for MariaDB
// ######################################

resource mariaDbServer01 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-01'
  location: 'westeurope'
  sku: {
    name: 'B_Gen5_1'
    tier: 'Basic'
    capacity: '1'
    size: '5120' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '5120'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }
}

resource mariaDbServer02 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-02'
  location: 'westeurope'
  sku: {
    name: 'B_Gen5_2'
    tier: 'Basic'
    capacity: '2'
    size: '5120' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '5120'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }
}

// ################################################
// General Purpose SKU - Azure Database for MariaDB
// ################################################

resource vnetExternal 'Microsoft.Network/virtualNetworks@2020-06-01' existing = {
  name: 'ace-vnet'
}

resource mariaDbServer03 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-03'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5_2'
    tier: 'GeneralPurpose'
    capacity: '2'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-03'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer04 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-04'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5_4'
    tier: 'GeneralPurpose'
    capacity: '4'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-04'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer05 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-05'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5_8'
    tier: 'GeneralPurpose'
    capacity: '8'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-05'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer06 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-06'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5_16'
    tier: 'GeneralPurpose'
    capacity: '16'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-06'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer07 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-07'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5_32'
    tier: 'GeneralPurpose'
    capacity: '32'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-07'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer08 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-08'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5_64'
    tier: 'GeneralPurpose'
    capacity: '64'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-08'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

// ################################################
// Memory Optimzed SKU - Azure Database for MariaDB
// ################################################

resource mariaDbServer09 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-09'
  location: 'westeurope'
  sku: {
    name: 'MO_Gen5_2'
    tier: 'MemoryOptimized'
    capacity: '2'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-09'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer10 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-10'
  location: 'westeurope'
  sku: {
    name: 'MO_Gen5_4'
    tier: 'MemoryOptimized'
    capacity: '4'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-10'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer11 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-11'
  location: 'westeurope'
  sku: {
    name: 'MO_Gen5_8'
    tier: 'MemoryOptimized'
    capacity: '8'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-11'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer12 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-12'
  location: 'westeurope'
  sku: {
    name: 'MO_Gen5_16'
    tier: 'MemoryOptimized'
    capacity: '16'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-12'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer13 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-13'
  location: 'westeurope'
  sku: {
    name: 'MO_Gen5_32'
    tier: 'MemoryOptimized'
    capacity: '32'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Disabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-13'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}

resource mariaDbServer14 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: 'ace-mdb-srv-14'
  location: 'westeurope'
  sku: {
    name: 'MO_Gen5_32'
    tier: 'MemoryOptimized'
    capacity: '32'
    size: '51200' //a string is expected here but a int for the storageProfile...
    family: 'Gen5'
  }
  properties: {
    createMode: 'Default'
    version: '10.3'
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@54321'
    storageProfile: {
      storageMB: '51200'
      backupRetentionDays: '7'
      geoRedundantBackup: 'Enabled'
    }
  }

  resource virtualNetworkRule 'virtualNetworkRules@2018-06-01' = {
    name: 'ace-mdb-srv-rule-14'
    properties: {
      virtualNetworkSubnetId: '${vnetExternal.id}/subnets/devSubnet'
      ignoreMissingVnetServiceEndpoint: true
    }
  }
}
