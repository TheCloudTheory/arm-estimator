param rgLocation string = resourceGroup().location
param redundancyMode string
param resourceNamePrefix string

metadata aceUsagePatterns = {
  Microsoft_RecoveryServices_vaults_Data_Stored: '50'
}

resource sg 'Microsoft.Network/networkSecurityGroups@2020-06-01' = {
  name: '${resourceNamePrefix}-nsg'
  location: rgLocation
  properties: {
    securityRules: [
      {
        name: 'default-allow-ssh' //Do not use following rule in production.
        'properties': {
          priority: 1000
          access: 'Allow'
          direction: 'Inbound'
          destinationPortRange: '22'
          protocol: 'Tcp'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
    ]
  }
}

resource vn 'Microsoft.Network/virtualNetworks@2020-06-01' = {
  name: '${resourceNamePrefix}-vnet'
  location: rgLocation
  properties: {
    addressSpace: {
      addressPrefixes: [
        '172.16.0.0/21'
      ]
    }
    subnets: [
      {
        name: 'devSubnet'
        properties: {
          addressPrefix: '172.16.0.0/24'
          networkSecurityGroup: {
            id: sg.id
          }
        }
      }
    ]
  }
}

resource recoveryServicesVault 'Microsoft.RecoveryServices/vaults@2020-02-02' = {
  name: '${resourceNamePrefix}-vault'
  location: rgLocation
  sku: {
    name: 'RS0'
    tier: 'Standard'
  }
  properties: {}
}

// ##############################################
// Setup Backup for Azure VM with Default Policy
// ##############################################

resource pip01 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: '${resourceNamePrefix}-pip-01'
  location: rgLocation
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}

resource nInter01 'Microsoft.Network/networkInterfaces@2020-06-01' = {
  name: '${resourceNamePrefix}-nic-01'
  location: rgLocation

  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: pip01.id
          }
          subnet: {
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', vn.name, 'devSubnet')
          }
        }
      }
    ]
  }
}

resource VM01 'Microsoft.Compute/virtualMachines@2020-06-01' = {
  name: '${resourceNamePrefix}-vm-01'
  location: rgLocation
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A1_v2'
    }
    osProfile: {
      computerName: 'acedevtest01'
      adminUsername: 'DemoUser'
      adminPassword: 'AzureTest@54321'
    }
    storageProfile: {
      imageReference: {
        publisher: 'Canonical'
        offer: 'UbuntuServer'
        sku: '18.04-LTS'
        version: 'latest'
      }
      osDisk: {
        createOption: 'FromImage'
      }
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: nInter01.id
        }
      ]
    }
  }
}

resource vaultName_backupFabric_protectionContainer_protectedItem_01 'Microsoft.RecoveryServices/vaults/backupFabrics/protectionContainers/protectedItems@2020-02-02' = {
  name: '${resourceNamePrefix}-vault/Azure/${'iaasvmcontainer;iaasvmcontainerv2;${resourceGroup().name};ace-vm-01'}/${'vm;iaasvmcontainerv2;${resourceGroup().name};ace-vm-01'}'
  properties: {
    protectedItemType: 'Microsoft.Compute/virtualMachines'
    policyId: '${recoveryServicesVault.id}/backupPolicies/DefaultPolicy'
    sourceResourceId: VM01.id
  }

}

// ##############################################
// Setup Backup for Azure VM with Custom Policy
// ##############################################

resource pip02 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: '${resourceNamePrefix}-pip-02'
  location: rgLocation
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}

resource nInter02 'Microsoft.Network/networkInterfaces@2020-06-01' = {
  name: '${resourceNamePrefix}-nic-02'
  location: rgLocation

  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig2'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: pip02.id
          }
          subnet: {
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', vn.name, 'devSubnet')
          }
        }
      }
    ]
  }
}

resource VM02 'Microsoft.Compute/virtualMachines@2020-06-01' = {
  name: '${resourceNamePrefix}-vm-02'
  location: rgLocation
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A2_v2'
    }
    osProfile: {
      computerName: 'acedevtest01'
      adminUsername: 'DemoUser'
      adminPassword: 'AzureTest@54321'
    }
    storageProfile: {
      imageReference: {
        publisher: 'Canonical'
        offer: 'UbuntuServer'
        sku: '18.04-LTS'
        version: 'latest'
      }
      osDisk: {
        createOption: 'FromImage'
      }
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: nInter02.id
        }
      ]
    }
  }
}

// Weekly Backup on Every Sunday at 10:00 PM EST and Retain instant recovery snapshots for 5 days
resource rRecoveryServiceVaultBackupPolicy 'Microsoft.RecoveryServices/vaults/backupPolicies@2021-03-01' = {
  name: '${resourceNamePrefix}-vault/${resourceNamePrefix}-backup-policy-01'
  properties: {
    backupManagementType: 'AzureIaasVM'
    instantRpRetentionRangeInDays: 5
    timeZone: 'Eastern Standard Time'
    protectedItemsCount: 0
    schedulePolicy: {
      schedulePolicyType: 'SimpleSchedulePolicy'
      scheduleRunFrequency: 'Weekly'
      scheduleRunDays: [
        'Sunday'
      ]
      scheduleRunTimes: [
        '2022-08-21T22:00:00Z'
      ]
      scheduleWeeklyFrequency: 0
    }
    retentionPolicy: {
      retentionPolicyType: 'LongTermRetentionPolicy'
      weeklySchedule: {
        daysOfTheWeek: [
          'Sunday'
        ]
        retentionTimes: [
          '2022-08-21T22:00:00Z'
        ]
        retentionDuration: {
          count: 12
          durationType: 'Weeks'
        }
      }
    }
  }

  dependsOn: [
    recoveryServicesVault
  ]
}

resource vaultName_backupFabric_protectionContainer_protectedItem_02 'Microsoft.RecoveryServices/vaults/backupFabrics/protectionContainers/protectedItems@2020-02-02' = {
  name: '${resourceNamePrefix}-vault/Azure/${'iaasvmcontainer;iaasvmcontainerv2;${resourceGroup().name};${resourceNamePrefix}-vm-02'}/${'vm;iaasvmcontainerv2;${resourceGroup().name};${resourceNamePrefix}-vm-02'}'
  properties: {
    protectedItemType: 'Microsoft.Compute/virtualMachines'
    policyId: '${recoveryServicesVault.id}/backupPolicies/ace-backup-policy-01'
    sourceResourceId: VM02.id
  }

  dependsOn: [
    rRecoveryServiceVaultBackupPolicy
  ]
}

resource vaultName_vaultstorageconfig 'Microsoft.RecoveryServices/vaults/backupstorageconfig@2022-02-01' = {
  parent: recoveryServicesVault
  name: 'vaultstorageconfig'
  properties: {
    storageModelType: redundancyMode
    crossRegionRestoreFlag: false
  }
}
