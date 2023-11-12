resource wafPolicy 'Microsoft.Network/FrontDoorWebApplicationFirewallPolicies@2019-03-01' = {
  name: 'ACEwafRule01'
  location: 'global'
  properties: {
    policySettings: {
      mode: 'Detection'
      enabledState: 'Enabled'
    }
    managedRules: {
      managedRuleSets: [
        {
          ruleSetType: 'DefaultRuleSet'
          ruleSetVersion: '1.0'
        }
      ]
    }
  }
}

resource frontDoor01 'Microsoft.Network/frontDoors@2020-05-01' = {
  name: 'aceAFD01'
  location: 'global'
  properties: {
    enabledState: 'Enabled'

    frontendEndpoints: [
      {
        name: 'ace-fe01'
        properties: {
          hostName: 'aceAFD01.azurefd.net'
          sessionAffinityEnabledState: 'Disabled'
          webApplicationFirewallPolicyLink: {
            id: resourceId('Microsoft.Network/frontdoorWebApplicationFirewallPolicies', 'ACEwafRule01')
          }
        }
      }
    ]

    loadBalancingSettings: [
      {
        name: 'acelbs01'
        properties: {
          sampleSize: 4
          successfulSamplesRequired: 2
        }
      }
    ]

    healthProbeSettings: [
      {
        name: 'acelbhc01'
        properties: {
          path: '/'
          protocol: 'Http'
          intervalInSeconds: 120
        }
      }
    ]

    backendPools: [
      {
        name: 'acebp01'
        properties: {
          backends: [
            {
              address: 'aceweb001.westeurope.cloudapp.azure.com'
              backendHostHeader: 'aceweb001.westeurope.cloudapp.azure.com'
              httpPort: 80
              httpsPort: 443
              weight: 50
              priority: 1
              enabledState: 'Enabled'
            }
          ]
          loadBalancingSettings: {
            id: resourceId('Microsoft.Network/frontDoors/loadBalancingSettings', 'aceAFD01', 'acelbs01')
          }
          healthProbeSettings: {
            id: resourceId('Microsoft.Network/frontDoors/healthProbeSettings', 'aceAFD01', 'acelbhc01')
          }
        }
      }
    ]

    routingRules: [
      {
        name: 'acert01'
        properties: {
          frontendEndpoints: [
            {
              id: resourceId('Microsoft.Network/frontDoors/frontEndEndpoints', 'aceAFD01', 'ace-fe01')
            }
          ]
          acceptedProtocols: [
            'Http'
            'Https'
          ]
          patternsToMatch: [
            '/*'
          ]
          routeConfiguration: {
            '@odata.type': '#Microsoft.Azure.FrontDoor.Models.FrontdoorForwardingConfiguration'
            forwardingProtocol: 'MatchRequest'
            backendPool: {
              id: resourceId('Microsoft.Network/frontDoors/backEndPools', 'aceAFD01', 'acebp01')
            }
          }
          enabledState: 'Enabled'
        }
      }
    ]
  }
}

// #####################
// WAF with Custom Rule
// #####################

resource wafPolicyName_resource 'Microsoft.Network/FrontDoorWebApplicationFirewallPolicies@2019-03-01' = {
  name: 'ACEwafRule02'
  location: 'global'
  properties: {
    policySettings: {
      mode: 'Detection'
      enabledState: 'Enabled'
    }
    customRules: {
      rules: [
        {
          name: 'Rule1'
          priority: 1
          enabledState: 'Enabled'
          ruleType: 'MatchRule'
          matchConditions: [
            {
              matchVariable: 'RemoteAddr'
              operator: 'IPMatch'
              matchValue: [
                '10.20.0.0/24'
              ]
            }
          ]
          action: 'Block'
        }
      ]
    }
  }
}

resource frontDoor02 'Microsoft.Network/frontDoors@2020-05-01' = {
  name: 'aceAFD02'
  location: 'global'
  properties: {
    enabledState: 'Enabled'

    frontendEndpoints: [
      {
        name: 'ace-fe02'
        properties: {
          hostName: 'aceAFD02.azurefd.net'
          sessionAffinityEnabledState: 'Disabled'
          webApplicationFirewallPolicyLink: {
            id: resourceId('Microsoft.Network/frontdoorWebApplicationFirewallPolicies', 'ACEwafRule02')
          }
        }
      }
    ]

    loadBalancingSettings: [
      {
        name: 'acelbs02'
        properties: {
          sampleSize: 4
          successfulSamplesRequired: 2
        }
      }
    ]

    healthProbeSettings: [
      {
        name: 'acelbhc02'
        properties: {
          path: '/'
          protocol: 'Http'
          intervalInSeconds: 120
        }
      }
    ]

    backendPools: [
      {
        name: 'acebp02'
        properties: {
          backends: [
            {
              address: 'aceweb001.westeurope.cloudapp.azure.com'
              backendHostHeader: 'aceweb001.westeurope.cloudapp.azure.com'
              httpPort: 80
              httpsPort: 443
              weight: 50
              priority: 1
              enabledState: 'Enabled'
            }
          ]
          loadBalancingSettings: {
            id: resourceId('Microsoft.Network/frontDoors/loadBalancingSettings', 'aceAFD02', 'acelbs02')
          }
          healthProbeSettings: {
            id: resourceId('Microsoft.Network/frontDoors/healthProbeSettings', 'aceAFD02', 'acelbhc02')
          }
        }
      }
    ]

    routingRules: [
      {
        name: 'acert02'
        properties: {
          frontendEndpoints: [
            {
              id: resourceId('Microsoft.Network/frontDoors/frontEndEndpoints', 'aceAFD02', 'ace-fe02')
            }
          ]
          acceptedProtocols: [
            'Http'
            'Https'
          ]
          patternsToMatch: [
            '/*'
          ]
          routeConfiguration: {
            '@odata.type': '#Microsoft.Azure.FrontDoor.Models.FrontdoorForwardingConfiguration'
            forwardingProtocol: 'MatchRequest'
            backendPool: {
              id: resourceId('Microsoft.Network/frontDoors/backEndPools', 'aceAFD02', 'acebp02')
            }
          }
          enabledState: 'Enabled'
        }
      }
    ]
  }
}
