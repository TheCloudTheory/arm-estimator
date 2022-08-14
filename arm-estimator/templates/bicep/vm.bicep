resource vm1 'Microsoft.Compute/virtualMachines@2022-03-01' = {
  name: 'vm1'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Basic_A0'
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

// resource vm2 'Microsoft.Compute/virtualMachines@2022-03-01' = {
//   name: 'vm2'
//   location: 'westeurope'
//   properties: {
//     hardwareProfile: {
//       vmSize: 'Basic_A1'
//     }
//     osProfile: { 
//       adminPassword: '123454567'
//       adminUsername: 'armestimator'   
//     }
//     storageProfile: {
//       imageReference: {
//         offer: 'WindowsServer'
//         publisher: 'MicrosoftWindowsServer'
//         sku: '2012-R2-Datacenter'
//         version: 'latest'
//       }
//     }
//   }
// }

// resource vm3 'Microsoft.Compute/virtualMachines@2022-03-01' = {
//   name: 'vm3'
//   location: 'westeurope'
//   properties: {
//     hardwareProfile: {
//       vmSize: 'Basic_A2'
//     }
//     osProfile: { 
//       adminPassword: '123454567'
//       adminUsername: 'armestimator'   
//     }
//     storageProfile: {
//       imageReference: {
//         offer: 'WindowsServer'
//         publisher: 'MicrosoftWindowsServer'
//         sku: '2012-R2-Datacenter'
//         version: 'latest'
//       }
//     }
//   }
// }

// resource vm4 'Microsoft.Compute/virtualMachines@2022-03-01' = {
//   name: 'vm4'
//   location: 'westeurope'
//   properties: {
//     hardwareProfile: {
//       vmSize: 'Basic_A3'
//     }
//     osProfile: { 
//       adminPassword: '123454567'
//       adminUsername: 'armestimator'   
//     }
//     storageProfile: {
//       imageReference: {
//         offer: 'WindowsServer'
//         publisher: 'MicrosoftWindowsServer'
//         sku: '2012-R2-Datacenter'
//         version: 'latest'
//       }
//     }
//   }
// }

resource vm5 'Microsoft.Compute/virtualMachines@2022-03-01' = {
  name: 'vm5'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Basic_A4'
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
