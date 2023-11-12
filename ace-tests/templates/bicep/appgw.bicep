resource vnet 'Microsoft.Network/virtualNetworks@2022-01-01' = {
  name: 'vnet'
  location: 'westeurope'
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.0/16'
      ]
    }
    subnets: [
      {
        name: 'subnet'
        properties: {
          addressPrefix: '10.0.0.0/27'
        }
      }
    ]
  }
}

resource appgw1 'Microsoft.Network/applicationGateways@2022-01-01' = {
  name: 'appgw1'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard_Small'
      capacity: 1
    }
    gatewayIPConfigurations: [
      {
        name: 'ipconfiguration'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'fip'
        properties: {
          
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'pool'
        properties: {
          
        }
      }
    ]
  }
}

resource appgw11 'Microsoft.Network/applicationGateways@2022-01-01' = {
  name: 'appgw11'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard_Small'
      capacity: 2
    }
    gatewayIPConfigurations: [
      {
        name: 'ipconfiguration'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'fip'
        properties: {
          
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'pool'
        properties: {
          
        }
      }
    ]
  }
}

resource appgw2 'Microsoft.Network/applicationGateways@2022-01-01' = {
  name: 'appgw2'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard_Medium'
      capacity: 1
    }
    gatewayIPConfigurations: [
      {
        name: 'ipconfiguration'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'fip'
        properties: {
          
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'pool'
        properties: {
          
        }
      }
    ]
  }
}

resource appgw3 'Microsoft.Network/applicationGateways@2022-01-01' = {
  name: 'appgw3'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard_Large'
      capacity: 1
    }
    gatewayIPConfigurations: [
      {
        name: 'ipconfiguration'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'fip'
        properties: {
          
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'pool'
        properties: {
          
        }
      }
    ]
  }
}

resource appgw4 'Microsoft.Network/applicationGateways@2022-01-01' = {
  name: 'appgw4'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard_v2'
      capacity: 1
      tier: 'Standard_v2'
    }
    gatewayIPConfigurations: [
      {
        name: 'ipconfiguration'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'fip'
        properties: {
          
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'pool'
        properties: {
          
        }
      }
    ]
  }
}

resource appgw44 'Microsoft.Network/applicationGateways@2022-01-01' = {
  name: 'appgw44'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard_v2'
      capacity: 2
      tier: 'Standard_v2'
    }
    gatewayIPConfigurations: [
      {
        name: 'ipconfiguration'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'fip'
        properties: {
          
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'pool'
        properties: {
          
        }
      }
    ]
  }
}

resource appgw5 'Microsoft.Network/applicationGateways@2022-01-01' = {
  name: 'appgw5'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'WAF_Medium'
      capacity: 1
      tier: 'WAF'
    }
    gatewayIPConfigurations: [
      {
        name: 'ipconfiguration'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'fip'
        properties: {
          
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'pool'
        properties: {
          
        }
      }
    ]
  }
}

resource appgw6 'Microsoft.Network/applicationGateways@2022-01-01' = {
  name: 'appgw6'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'WAF_Large'
      capacity: 1
      tier: 'WAF'
    }
    gatewayIPConfigurations: [
      {
        name: 'ipconfiguration'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'fip'
        properties: {
          
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'pool'
        properties: {
          
        }
      }
    ]
  }
}

resource appgw7 'Microsoft.Network/applicationGateways@2022-01-01' = {
  name: 'appgw7'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'WAF_v2'
      capacity: 1
      tier: 'WAF_v2'
    }
    gatewayIPConfigurations: [
      {
        name: 'ipconfiguration'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'fip'
        properties: {
          
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'pool'
        properties: {
          
        }
      }
    ]
  }
}
