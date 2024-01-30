param parLocation string = resourceGroup().location
param parSuffix string = utcNow('yyyyMMddhhmmss')

resource vm 'Microsoft.Compute/virtualMachines@2023-09-01' = {
#disable-next-line use-stable-resource-identifiers
  name: 'vm-${parSuffix}'
  location: parLocation
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_D2s_v3'
    }
    osProfile: { 
      adminPassword: '123454567'
#disable-next-line adminusername-should-not-be-literal
      adminUsername: 'armestimator'   
    }
    storageProfile: {
      imageReference: {
        offer: '0001-com-ubuntu-server-jammy'
        publisher: 'canonical'
        sku: '22_04-lts-gen2'
        version: 'latest'
      }
    }
  }
}

resource asr 'Microsoft.RecoveryServices/vaults@2023-06-01' = {
#disable-next-line use-stable-resource-identifiers
  name: 'asr-${parSuffix}'
  location: parLocation
  sku: {
    name: 'Standard'
  }
  properties: {}
}

resource rp 'Microsoft.RecoveryServices/vaults/replicationPolicies@2023-06-01' = {
  name: '24-hour-retention-policy'
  parent: asr
  properties: {}
}

resource rpi 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers/replicationProtectedItems@2023-06-01' = {
  name: guid('rpi')
  parent: rpc
  properties: {
     policyId: rp.id
     providerSpecificDetails: {
      fabricObjectId: vm.id
      instanceType: 'A2A'
     }
  }
}

resource rpcm 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers/replicationProtectionContainerMappings@2023-06-01' = {
  name: 'westeurope-westus-24-hour-retention-policy'
  parent: rpc
  properties: {
     policyId: rp.id
     targetProtectionContainerId: rpc.id
  }
}

resource rpc 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers@2023-06-01' = {
  name: 'asr-a2a-default-westus-container'
  parent: rf
  properties: {}
}

resource rf 'Microsoft.RecoveryServices/vaults/replicationFabrics@2023-06-01' = {
  name: 'asr-a2a-default-westus'
  parent: asr
  properties: {
    customDetails: {
      instanceType: 'Azure'
      location: 'westus'
    }
  }
}
