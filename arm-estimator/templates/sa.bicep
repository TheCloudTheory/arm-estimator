resource sa1 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'armestimatorsa1'
  location: 'westeurope'
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource sa2 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'armestimatorsa2'
  location: 'westeurope'
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Cool'
  }
}

resource sa3 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'armestimatorsa3'
  location: 'westeurope'
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Premium'
  }
}
