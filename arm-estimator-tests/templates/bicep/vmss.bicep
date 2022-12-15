resource vmss 'Microsoft.Compute/virtualMachineScaleSets@2022-08-01' = {
  name: 'vmss'
  location: resourceGroup().location
  sku: {
    capacity: 3
    name: 'Standard_A0'
    tier: 'Standard'
  }
  properties: {
    virtualMachineProfile: {   
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
}
