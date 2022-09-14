resource sa 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: 'nested1'
  location: resourceGroup().location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
  }

  resource blobServices 'blobServices@2022-05-01' = {
    name: 'default'
    properties: {
    }

    resource container 'containers@2022-05-01' = {
      name: 'private'
      properties: {
       publicAccess: 'None' 
      }
    }
  }
}
