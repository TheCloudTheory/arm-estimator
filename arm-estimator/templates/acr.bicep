resource acr 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: 'armestimatoracr'
  location: 'westeurope'
  sku: {
    name: 'Basic'
  }
}
