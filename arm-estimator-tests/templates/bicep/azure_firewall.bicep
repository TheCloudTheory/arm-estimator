resource workloadIpGroup 'Microsoft.Network/ipGroups@2022-01-01' = {
  name: 'ace-workloadIpGroup-01'
  location: 'westeurope'
  properties: {
    ipAddresses: [
      '10.20.0.0/24'
      '10.30.0.0/24'
    ]
  }
}

resource firewallPolicy 'Microsoft.Network/firewallPolicies@2022-01-01'= {
  name: 'ace-firewallPolicy'
  location: 'westeurope'
  properties: {
    threatIntelMode: 'Alert'
  }
}

resource networkRuleCollectionGroup 'Microsoft.Network/firewallPolicies/ruleCollectionGroups@2022-01-01' = {
  parent: firewallPolicy
  name: 'DefaultNetworkRuleCollectionGroup'
  properties: {
    priority: 200
    ruleCollections: [
      {
        ruleCollectionType: 'FirewallPolicyFilterRuleCollection'
        action: {
          type: 'Allow'
        }
        name: 'azure-global-services-nrc'
        priority: 1250
        rules: [
          {
            ruleType: 'NetworkRule'
            name: 'time-windows'
            ipProtocols: [
              'UDP'
            ]
            destinationAddresses: [
              '13.86.101.172'
            ]
            sourceIpGroups: [
              workloadIpGroup.id
            ]
            destinationPorts: [
              '123'
            ]
          }
        ]
      }
    ]
  }
}

resource applicationRuleCollectionGroup 'Microsoft.Network/firewallPolicies/ruleCollectionGroups@2022-01-01' = {
  parent: firewallPolicy
  name: 'DefaultApplicationRuleCollectionGroup'
  dependsOn: [
    networkRuleCollectionGroup
  ]
  properties: {
    priority: 300
    ruleCollections: [
      {
        ruleCollectionType: 'FirewallPolicyFilterRuleCollection'
        name: 'global-rule-url-arc'
        priority: 1000
        action: {
          type: 'Allow'
        }
        rules: [
          {
            ruleType: 'ApplicationRule'
            name: 'winupdate-rule-01'
            protocols: [
              {
                protocolType: 'Https'
                port: 443
              }
              {
                protocolType: 'Http'
                port: 80
              }
            ]
            fqdnTags: [
              'WindowsUpdate'
            ]
            terminateTLS: false
            sourceIpGroups: [
              workloadIpGroup.id
            ]
          }
        ]
      }
      {
        ruleCollectionType: 'FirewallPolicyFilterRuleCollection'
        action: {
          type: 'Allow'
        }
        name: 'Global-rules-arc'
        priority: 1202
        rules: [
          {
            ruleType: 'ApplicationRule'
            name: 'global-rule-01'
            protocols: [
              {
                protocolType: 'Https'
                port: 443
              }
            ]
            targetFqdns: [
              'www.microsoft.com'
            ]
            terminateTLS: false
            sourceIpGroups: [
              workloadIpGroup.id
            ]
          }
        ]
      }
    ]
  }
}

resource publicIpAddress01 'Microsoft.Network/publicIPAddresses@2022-01-01' = {
  name: 'ace-fw-pip-01'
  location: 'westeurope'
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
    publicIPAddressVersion: 'IPv4'
  }
  zones: [
    '1' , '2'
  ]
}

resource firewall01 'Microsoft.Network/azureFirewalls@2021-03-01' = {
  name: 'ace-fw-01'
  location: 'westeurope'
  zones: ['1', '2']
  dependsOn: [
    publicIpAddress01
    workloadIpGroup
    networkRuleCollectionGroup
    applicationRuleCollectionGroup
  ]
  properties: {
    ipConfigurations: [
      {
        name: 'FirewallIPConfiguration01'
        properties: {
          subnet: {
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', 'ace-vnet', 'AzureFirewallSubnet')
          }
          publicIPAddress: {
            id: publicIpAddress01.id
          }
        }
      }
    ]
    firewallPolicy: {
      id: firewallPolicy.id
    }
  }
}

resource publicIpAddress02 'Microsoft.Network/publicIPAddresses@2022-01-01' = {
  name: 'ace-fw-pip-02'
  location: 'westeurope'
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
    publicIPAddressVersion: 'IPv4'
  }
  zones: [
    '1' , '2'
  ]
}

resource firewallPolicy02 'Microsoft.Network/firewallPolicies@2022-01-01' = {
  name: 'DemoFirewallPolicy'
  location: 'westeurope'
  properties: {
    sku: {
      tier: 'Premium'
    }
    threatIntelMode: 'Alert'
  }
}

resource firewall02 'Microsoft.Network/azureFirewalls@2021-03-01' = {
  name: 'ace-fw-02'
  location: 'westeurope'
  zones: ['1', '2']
  properties: {
    ipConfigurations: [
      {
        name: 'FirewallIPConfiguration02'
        properties: {
          subnet: {
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', 'ace-vnet-02', 'AzureFirewallSubnet')
          }
          publicIPAddress: {
            id: publicIpAddress02.id
          }
        }
      }
    ]
    firewallPolicy: {
      id: firewallPolicy02.id
    }
    sku: {
      name: 'AZFW_VNet'
      tier: 'Premium'
    }
  }
  dependsOn: [
    publicIpAddress02
  ]
}
