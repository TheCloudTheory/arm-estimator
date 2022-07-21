resource acr 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: 'armestimatoracr'
  location: 'westeurope'
  sku: {
    name: 'Basic'
  }
}

resource acr2 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: 'armestimatoracr2'
  location: 'westeurope'
  sku: {
    name: 'Standard'
  }
}

resource acr3 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: 'armestimatoracr3'
  location: 'westeurope'
  sku: {
    name: 'Premium'
  }
}
