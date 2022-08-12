resource containerapp 'Microsoft.App/containerApps@2022-03-01' = {
  name: 'containerapp'
  location: 'westeurope'
  properties: {
  }
}

resource dbserver 'Microsoft.Sql/servers@2021-11-01-preview' = {
  name: 'sqlserver'
  location: 'westeurope'
  properties: {
  }
}

resource dbbasic 'Microsoft.Sql/servers/databases@2021-11-01-preview' = {
  parent: dbserver
  name: 'dbbasic'
  location: 'westeurope'
  sku: {
    name: 'Basic'
  }
}

resource dbstandard0 'Microsoft.Sql/servers/databases@2021-11-01-preview' = {
  parent: dbserver
  name: 'dbstandard0'
  location: 'westeurope'
  sku: {
    name: 'S0'
  }
}

resource dbstandard1 'Microsoft.Sql/servers/databases@2021-11-01-preview' = {
  parent: dbserver
  name: 'dbstandard1'
  location: 'westeurope'
  sku: {
    name: 'S1'
  }
}

resource dbstandard2 'Microsoft.Sql/servers/databases@2021-11-01-preview' = {
  parent: dbserver
  name: 'dbstandard2'
  location: 'westeurope'
  sku: {
    name: 'S2'
  }
}

resource dbstandard3 'Microsoft.Sql/servers/databases@2021-11-01-preview' = {
  parent: dbserver
  name: 'dbstandard3'
  location: 'westeurope'
  sku: {
    name: 'S3'
  }
}

resource dbstandard4 'Microsoft.Sql/servers/databases@2021-11-01-preview' = {
  parent: dbserver
  name: 'dbstandard4'
  location: 'westeurope'
  sku: {
    name: 'S4'
  }
}

resource sa2 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'armestimatorsa2'
  location: 'westeurope'
  sku: {
    name: 'Standard_GRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Cool'
  }
}
