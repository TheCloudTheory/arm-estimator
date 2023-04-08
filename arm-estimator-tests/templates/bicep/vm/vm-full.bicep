param vmSize string = 'Standard_B1s'
param vmName string = 'acevmfull'
param nicName string = 'acenic'
param location string = resourceGroup().location

resource vm 'Microsoft.Compute/virtualMachines@2022-03-01' = {
  name: vmName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: vmSize
    }
    osProfile: { 
      adminPassword: '&7sd785asd5aHgsdf'
      adminUsername: 'armestimator'   
      computerName: 'ACE'
    }
    storageProfile: {
      imageReference: {
        offer: 'WindowsServer'
        publisher: 'MicrosoftWindowsServer'
        sku: '2012-R2-Datacenter'
        version: 'latest'
      }
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: nic.id
        }
      ]
    }
  }
}

resource nic 'Microsoft.Network/networkInterfaces@2022-09-01' = {
  name: nicName
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'ip'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
  }
}

resource vnet 'Microsoft.Network/virtualNetworks@2022-07-01' = {
  name: 'acevnet'
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.0/24'
      ]
    }
    subnets: [
      {
        name: 'default'
        properties: {
          addressPrefix: '10.0.0.0/28'
        }
      }
    ]
  }
}
