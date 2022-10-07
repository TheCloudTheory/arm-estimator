resource networkSecurityGroup 'Microsoft.Network/networkSecurityGroups@2021-08-01' = {
  name: 'ace-nsg-01'
  location: 'westeurope'
  properties: {
    securityRules: [
      {
        name: 'allow_tds_inbound'
        properties: {
          description: 'Allow access to data'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '1433'
          sourceAddressPrefix: 'VirtualNetwork'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 1000
          direction: 'Inbound'
        }
      }
      {
        name: 'allow_redirect_inbound'
        properties: {
          description: 'Allow inbound redirect traffic to Managed Instance inside the virtual network'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '11000-11999'
          sourceAddressPrefix: 'VirtualNetwork'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 1100
          direction: 'Inbound'
        }
      }
      {
        name: 'deny_all_inbound'
        properties: {
          description: 'Deny all other inbound traffic'
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Deny'
          priority: 4096
          direction: 'Inbound'
        }
      }
      {
        name: 'deny_all_outbound'
        properties: {
          description: 'Deny all other outbound traffic'
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Deny'
          priority: 4096
          direction: 'Outbound'
        }
      }
    ]
  }
}

resource routeTable 'Microsoft.Network/routeTables@2021-08-01' = {
  name: 'ace-rt-01'
  location: 'westeurope'
  properties: {
    disableBgpRoutePropagation: false
  }
}

resource virtualNetwork 'Microsoft.Network/virtualNetworks@2019-11-01' = {
  name: 'ace-vnet-01'
  location: 'westeurope'
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.20.0.0/16'
      ]
    }
    subnets: [
      {
        name: 'acesub'
        properties: {
          addressPrefix: '10.20.0.0/24'
          routeTable: {
            id: routeTable.id
          }
          networkSecurityGroup: {
            id: networkSecurityGroup.id
          }
          delegations: [
            {
              name: 'managedInstanceDelegation'
              properties: {
                serviceName: 'Microsoft.Sql/managedInstances'
              }
            }
          ]
        }
      }
    ]
    
  }
}

resource managedInstance01 'Microsoft.Sql/managedInstances@2022-02-01-preview' = {
  name: 'ace-sqlmi-01'
  location: 'westeurope'
  sku: {
    name: 'GP_Gen5'
    tier: 'GeneralPurpose'
  }
  identity: {
    type: 'SystemAssigned'
  }
  dependsOn: [
    virtualNetwork
  ]
  properties: {
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    subnetId: resourceId('Microsoft.Network/virtualNetworks/subnets', 'ace-vnet-01', 'acesub')
    storageSizeInGB: 32
    vCores: 8
    licenseType: 'LicenseIncluded'
  }
}

resource managedInstance02 'Microsoft.Sql/managedInstances@2022-02-01-preview' = {
  name: 'ace-sqlmi-02'
  location: 'westeurope'
  sku: {
    name: 'BC_Gen5'
    tier: 'BusinessCritical'
  }
  identity: {
    type: 'SystemAssigned'
  }
  dependsOn: [
    virtualNetwork
  ]
  properties: {
    administratorLogin: 'DemoUser'
    administratorLoginPassword: 'AzureTest@654321'
    subnetId: resourceId('Microsoft.Network/virtualNetworks/subnets', 'ace-vnet-01', 'acesub')
    storageSizeInGB: 32
    vCores: 8
    licenseType: 'LicenseIncluded'
  }
}
