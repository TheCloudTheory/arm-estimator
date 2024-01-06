param parSuffix string = utcNow('yyyyMMddhhmmss')
param parLocation string = resourceGroup().location

resource aks 'Microsoft.ContainerService/managedClusters@2023-10-01' = {
#disable-next-line use-stable-resource-identifiers
  name: 'aks-${parSuffix}'
  location: parLocation
  sku: {
    tier: 'Free'
  }
  properties: {
    agentPoolProfiles: [
      {
        name: 'agentpool'
        type: 'AvailabilitySet'
        count: 1
        vmSize: 'Standard_D2s_v3'
        osDiskSizeGB: 4
        osType: 'Windows'
      }
    ]
  }
}
