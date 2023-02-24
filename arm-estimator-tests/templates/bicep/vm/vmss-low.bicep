param rgLocation string = resourceGroup().location
param adminUsername string = 'armestimator'

resource vmss 'Microsoft.Compute/virtualMachineScaleSets@2022-08-01' = {
  name: 'vmss'
  location: rgLocation
  sku: {
    capacity: 3
    name: 'Standard_A1_v2'
    tier: 'Standard'
  }
  properties: {
    virtualMachineProfile: {   
      osProfile: { 
        adminPassword: '123454567'
        adminUsername: adminUsername 
      }
      storageProfile: {
        imageReference: {
          offer: 'WindowsServer'
          publisher: 'MicrosoftWindowsServer'
          sku: '2012-R2-Datacenter'
          version: 'latest'
        }
      }
      priority: 'Low'
    }
  }
}
