param vmSize string
param vmName string

resource vm 'Microsoft.Compute/virtualMachines@2022-03-01' = {
  name: vmName
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
        offer: 'WindowsServer'
        publisher: 'MicrosoftWindowsServer'
        sku: '2012-R2-Datacenter'
        version: 'latest'
      }
    }
  }
}
