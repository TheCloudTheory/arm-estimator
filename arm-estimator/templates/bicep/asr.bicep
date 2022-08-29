// ###########################################################################################
// WARNING: The following ASR Template does not give the desired result in the first attempt, 
// -> You might need to Rerun template to get the desired outcome.
// -> Update all the values as per your requirement.
// ###########################################################################################

resource recoveryServicesVault 'Microsoft.RecoveryServices/vaults@2020-02-02' = {
  name: 'ace-vault'
  location: 'northeurope'
  sku: {
    name: 'RS0'
    tier: 'Standard'
  }
  properties: {}
}

resource sourceFabricName 'Microsoft.RecoveryServices/vaults/replicationFabrics@2022-03-01' = {
  name: 'ace-vault/westeurope-fabric'
  properties: {
    customDetails: {
      instanceType: 'Azure'
      location: 'westeurope'
    }
  }
  dependsOn: [
    recoveryServicesVault
  ]
}

resource targetFabricName 'Microsoft.RecoveryServices/vaults/replicationFabrics@2022-03-01' = {
  name: 'ace-vault/northeurope-fabric'
  properties: {
    customDetails: {
      instanceType: 'Azure'
      location: 'northeurope'
    }
  }
  dependsOn: [
    recoveryServicesVault
  ]
}

resource replicationPolicyName 'Microsoft.RecoveryServices/vaults/replicationPolicies@2018-07-10' = {
  name: 'ace-vault/ace-retentionpolicy'
  properties: {
    providerSpecificInput: {
      instanceType: 'A2A'
      appConsistentFrequencyInMinutes: 240
      crashConsistentFrequencyInMinutes: 5
      recoveryPointHistory: 1440
      multiVmSyncStatus: 'Enable'
    }
  }
  dependsOn: [
    recoveryServicesVault
  ]
}

resource sourceContainerName 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers@2022-03-01' = {
  name: 'ace-vault/westeurope-fabric/acewesteurope-container'
  properties: {
    providerSpecificInput: [
      {
        instanceType: 'A2A'
      }
    ]
  }
  dependsOn: [
    sourceFabricName
  ]
}

resource targetContainerName 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers@2022-03-01' = {
  name: 'ace-vault/northeurope-fabric/acenortheurope-container'
  properties: {
    providerSpecificInput: [
      {
        instanceType: 'A2A'
      }
    ]
  }
  dependsOn: [
    targetFabricName
  ]
}

resource sourceMapping 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers/replicationProtectionContainerMappings@2018-01-10' = {
  name: 'ace-vault/westeurope-fabric/acewesteurope-container/ace-westeurope-northeurope-24-hour-retention-policy'
  properties: {
    policyId: resourceId('Microsoft.RecoveryServices/vaults/replicationPolicies/', 'ace-vault', 'ace-retentionpolicy')
    targetProtectionContainerId: resourceId('Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers', 'ace-vault', 'northeurope-fabric', 'acenortheurope-container')
    providerSpecificInput: {
      instanceType: 'A2A'
    }
  }
  dependsOn: [ 
    recoveryServicesVault
    replicationPolicyName
    sourceFabricName
  ]
}

resource targetMapping 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers/replicationProtectionContainerMappings@2018-01-10' = {
  name: 'ace-vault/northeurope-fabric/acenortheurope-container/ace-northeurope-westeurope-24-hour-retention-policy'
  properties: {
    policyId: resourceId('Microsoft.RecoveryServices/vaults/replicationPolicies/', 'ace-vault', 'ace-retentionpolicy')
    targetProtectionContainerId: resourceId('Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers', 'ace-vault', 'westeurope-fabric', 'acewesteurope-container')
    providerSpecificInput: {
      instanceType: 'A2A'
    }
  }
  dependsOn: [
    recoveryServicesVault
    replicationPolicyName
    targetFabricName
   ]
}

resource sourceVnet 'Microsoft.Network/virtualNetworks@2020-06-01' existing = {
  name: 'ace-vnet'
}

resource destVnet 'Microsoft.Network/virtualNetworks@2020-06-01' existing = {
  name: 'ace-vnet-dr'
  scope: resourceGroup('azcost-dr')
}

resource sourceVNetMapping 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationNetworks/replicationNetworkMappings@2021-07-01' = {
  name: 'ace-vault/westeurope-fabric/azureNetwork/westeurope-northeurope-vnet-mapping'
  properties: {
    recoveryFabricName: 'northeurope-fabric'
    recoveryNetworkId: destVnet.id
    fabricSpecificDetails: {
      instanceType: 'AzureToAzure'
      primaryNetworkId: sourceVnet.id
    }
  }
  dependsOn: [ 
    recoveryServicesVault
    sourceFabricName
  ]
}

resource targetVNetMapping 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationNetworks/replicationNetworkMappings@2021-07-01' = {
  name: 'ace-vault/northeurope-fabric/azureNetwork/northeurope-westeurope-vnet-mapping'
  properties: {
    recoveryFabricName: 'westeurope-fabric'
    recoveryNetworkId: sourceVnet.id
    fabricSpecificDetails: {
      instanceType: 'AzureToAzure'
      primaryNetworkId: destVnet.id
    }
  }
  dependsOn: [
    recoveryServicesVault
    targetFabricName
   ]
}

resource stagingStorage 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: 'acestg28271'
  location: 'westeurope'
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource existVM 'Microsoft.Compute/virtualMachines@2020-06-01' existing = {
  name: 'ace-vm-01'
}

resource exDisk 'Microsoft.Compute/disks@2022-03-02' existing = {
  name: 'ace-vm-01_disk1_a5ff63ade61843f0bc904c5f537a05fb'
}

resource existavs 'Microsoft.Compute/availabilitySets@2022-03-01' existing = {
  name: 'ace-avail-set'
  scope: resourceGroup('azcost-dr')
}

resource destRg 'Microsoft.Resources/resourceGroups@2021-01-01' existing = {
  name: 'azcost-dr'
  scope: subscription()
}

@description('Array of VMs and its managed disks which needs to be replicated')

var existingVMs = [
  {
    vmId: existVM.id
    targetAvailabiltySetId: existavs.id
    managedDisks: [ exDisk.id ]
  }
]

resource sourceprotectionItemPrefix_vmloop 'Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers/replicationProtectedItems@2018-01-10' = [for (item, i) in existingVMs: {
  name: 'ace-vault/westeurope-fabric/acewesteurope-container/ace-protectedItems'
  properties: {
    policyId: resourceId('Microsoft.RecoveryServices/vaults/replicationPolicies/', 'ace-vault', 'ace-retentionpolicy')
    protectableItemId: ''
    providerSpecificDetails: {
      instanceType: 'A2A'
      fabricObjectId: item.vmId
      recoveryContainerId: resourceId('Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers', 'ace-vault', 'northeurope-fabric', 'acenortheurope-container')
      recoveryResourceGroupId: destRg.id
      vmManagedDisks: [for j in range(0, length(item.managedDisks)): {
        diskId: item.managedDisks[j]
        primaryStagingAzureStorageAccountId: stagingStorage.id
        recoveryResourceGroupId: resourceGroup().id
        recoveryReplicaDiskAccountType: 'Standard_LRS'
        recoveryTargetDiskAccountType: 'Standard_LRS'
      }]
      multiVmGroupName: 'ace-multivmgroup-01'
      recoveryAvailabilitySetId: item.targetAvailabiltySetId
    }
  }
  dependsOn: [
    sourceContainerName
    targetContainerName
    replicationPolicyName
    sourceMapping
    targetMapping
  ]
}]
