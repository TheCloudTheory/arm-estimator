resource aksuptime 'Microsoft.ContainerService/managedClusters@2022-05-02-preview' = {
  name: 'aksuptime'
  location: 'westeurope'
  sku: {
    name: 'Basic'
    tier: 'Paid'
  }
}

resource aks 'Microsoft.ContainerService/managedClusters@2022-05-02-preview' = {
  name: 'aks'
  location: 'westeurope'
  sku: {
    name: 'Basic'
  }
}

resource containerapp 'Microsoft.App/containerApps@2022-03-01' = {
  name: 'containerapp'
  location: 'westeurope'
  properties: {
    
  }
}
