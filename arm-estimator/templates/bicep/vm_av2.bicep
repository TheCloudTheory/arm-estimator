resource sg 'Microsoft.Network/networkSecurityGroups@2020-06-01' = {
  name: 'ace-nsg'
  location: 'westeurope'
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
  name: 'ace-vnet'
  location: 'westeurope'
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

resource pip01 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: 'ace-pip-01'
  location: 'westeurope'
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}

resource nInter01 'Microsoft.Network/networkInterfaces@2020-06-01' = {
  name: 'ace-nic-01'
  location: 'westeurope'

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
  name: 'ace-vm-01'
  location: 'westeurope'
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

// ------------- End of Section ---------------------

resource pip02 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: 'ace-pip-02'
  location: 'westeurope'
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}

resource nInter02 'Microsoft.Network/networkInterfaces@2020-06-01' = {
  name: 'ace-nic-02'
  location: 'westeurope'

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
  name: 'ace-vm-02'
  location: 'westeurope'
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

// ------------- End of Section ---------------------

resource pip03 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: 'ace-pip-03'
  location: 'westeurope'
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}

resource nInter03 'Microsoft.Network/networkInterfaces@2020-06-01' = {
  name: 'ace-nic-03'
  location: 'westeurope'

  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig3'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: pip03.id
          }
          subnet: {
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', vn.name, 'devSubnet')
          }
        }
      }
    ]
  }
}

resource VM03 'Microsoft.Compute/virtualMachines@2020-06-01' = {
  name: 'ace-vm-03'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A4_v2'
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
          id: nInter03.id
        }
      ]
    }
  }
}

// ------------- End of Section ---------------------

resource pip04 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: 'ace-pip-04'
  location: 'westeurope'
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}

resource nInter04 'Microsoft.Network/networkInterfaces@2020-06-01' = {
  name: 'ace-nic-04'
  location: 'westeurope'

  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig4'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: pip04.id
          }
          subnet: {
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', vn.name, 'devSubnet')
          }
        }
      }
    ]
  }
}

resource VM04 'Microsoft.Compute/virtualMachines@2020-06-01' = {
  name: 'ace-vm-04'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A8_v2'
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
          id: nInter04.id
        }
      ]
    }
  }
}

// ------------- End of Section ---------------------

resource pip05 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: 'ace-pip-05'
  location: 'westeurope'
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}

resource nInter05 'Microsoft.Network/networkInterfaces@2020-06-01' = {
  name: 'ace-nic-05'
  location: 'westeurope'

  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig5'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: pip05.id
          }
          subnet: {
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', vn.name, 'devSubnet')
          }
        }
      }
    ]
  }
}

resource VM05 'Microsoft.Compute/virtualMachines@2020-06-01' = {
  name: 'ace-vm-05'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A2m_v2'
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
          id: nInter05.id
        }
      ]
    }
  }
}

// ------------- End of Section ---------------------

resource pip06 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: 'ace-pip-06'
  location: 'westeurope'
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}

resource nInter06 'Microsoft.Network/networkInterfaces@2020-06-01' = {
  name: 'ace-nic-06'
  location: 'westeurope'

  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig6'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: pip06.id
          }
          subnet: {
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', vn.name, 'devSubnet')
          }
        }
      }
    ]
  }
}

resource VM06 'Microsoft.Compute/virtualMachines@2020-06-01' = {
  name: 'ace-vm-06'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A4m_v2'
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
          id: nInter06.id
        }
      ]
    }
  }
}

// ------------- End of Section ---------------------

resource pip07 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: 'ace-pip-07'
  location: 'westeurope'
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}

resource nInter07 'Microsoft.Network/networkInterfaces@2020-06-01' = {
  name: 'ace-nic-07'
  location: 'westeurope'

  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig7'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: pip07.id
          }
          subnet: {
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', vn.name, 'devSubnet')
          }
        }
      }
    ]
  }
}

resource VM07 'Microsoft.Compute/virtualMachines@2020-06-01' = {
  name: 'ace-vm-07'
  location: 'westeurope'
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_A8m_v2'
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
          id: nInter07.id
        }
      ]
    }
  }
}

// ------------- End of Section ---------------------
