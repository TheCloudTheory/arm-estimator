param vmSize string
param vmName string

resource vm 'Microsoft.Compute/virtualMachines@2022-03-01' = {
  name: '${vmName}-linux'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: vmSize
    }
    osProfile: { 
      adminPassword: '123454567'
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
