resource as1 'Microsoft.Compute/availabilitySets@2022-08-01' = {
  name: 'as'
  location: resourceGroup().location 
  properties: {
    virtualMachines: [
      vm1
      vm2
      vm3
    ]
  }
}

resource vm1 'Microsoft.Compute/virtualMachines@2022-03-01' = {
  name: 'vm1'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A0'
    }
    osProfile: { 
      adminPassword: '123454567'
      adminUsername: 'armestimator'   
    }
    storageProfile: {
      imageReference: {
        offer: 'WindowsServer'
        publisher: 'MicrosoftWindowsServer'
        sku: '2012-R2-Datacenter'
        version: 'latest'
      }
    }
  }
}

resource vm2 'Microsoft.Compute/virtualMachines@2022-03-01' = {
  name: 'vm2'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A1'
    }
    osProfile: { 
      adminPassword: '123454567'
      adminUsername: 'armestimator'   
    }
    storageProfile: {
      imageReference: {
        offer: 'WindowsServer'
        publisher: 'MicrosoftWindowsServer'
        sku: '2012-R2-Datacenter'
        version: 'latest'
      }
    }
  }
}

resource vm3 'Microsoft.Compute/virtualMachines@2022-03-01' = {
  name: 'vm3'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A2'
    }
    osProfile: { 
      adminPassword: '123454567'
      adminUsername: 'armestimator'   
    }
    storageProfile: {
      imageReference: {
        offer: 'WindowsServer'
        publisher: 'MicrosoftWindowsServer'
        sku: '2012-R2-Datacenter'
        version: 'latest'
      }
    }
  }
}
