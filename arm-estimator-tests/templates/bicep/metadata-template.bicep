metadata aceUsagePatterns = {
  Microsoft_ContainerRegistry_registries_Data_Stored: '50'
  Microsoft_ContainerRegistry_registries_Task_vCPU_Duration: '3600'
}

resource acr 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: 'metadataacr'
  location: resourceGroup().location
  sku: {
    name: 'Basic'
  }
}
