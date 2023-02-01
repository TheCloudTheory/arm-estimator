param name string
param location string = resourceGroup().location

resource acr 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' = {
  name:name
  location: location
  sku: {
    name: 'Basic'
  }
}
