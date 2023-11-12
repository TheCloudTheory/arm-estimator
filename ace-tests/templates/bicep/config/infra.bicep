param location string = resourceGroup().location

resource storage_standard_lrs 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: 'storagestandardlrs123'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}
