resource storage_standard_lrs 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'storagestandardlrs123'
  location: resourceGroup().location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource storage_standard_zrs 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'storagestandardzrs123'
  location: resourceGroup().location
  sku: {
    name: 'Standard_ZRS'
  }
  kind: 'StorageV2'
}

resource storage_standard_grs 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'storagestandardgrs123'
  location: resourceGroup().location
  sku: {
    name: 'Standard_GRS'
  }
  kind: 'StorageV2'
}

resource storage_standard_gzrs 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'storagestandardgzrs123'
  location: resourceGroup().location
  sku: {
    name: 'Standard_GZRS'
  }
  kind: 'StorageV2'
}

resource storage_standard_ragrs 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'storagestandragrs123'
  location: resourceGroup().location
  sku: {
    name: 'Standard_RAGRS'
  }
  kind: 'StorageV2'
}

resource storage_standard_ragzrs 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'storagestandragzrs123'
  location: resourceGroup().location
  sku: {
    name: 'Standard_RAGZRS'
  }
  kind: 'StorageV2'
}

resource storage_premium_lrs 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'storagepremiumlrs123'
  location: resourceGroup().location
  sku: {
    name: 'Premium_LRS'
  }
  kind: 'StorageV2'
}

resource storage_premium_zrs 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: 'storagepremiumzrs123'
  location: resourceGroup().location
  sku: {
    name: 'Premium_ZRS'
  }
  kind: 'StorageV2'
}
