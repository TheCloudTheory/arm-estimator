metadata aceUsagePatterns = {
  Microsoft_ContainerRegistry_registries_Task_vCPU_Duration: '7200'
}

param rgLocation string = resourceGroup().location
param acrName string

resource acr 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: acrName
  location: rgLocation
  sku: {
    name: 'Basic'
  }
}
