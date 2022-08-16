resource publicIPPrefix 'Microsoft.Network/publicIPPrefixes@2021-02-01' = {
  name: 'ace-pip-prefix'
  location: 'westeurope'
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
  properties: {
    prefixLength: '29'
    publicIPAddressVersion: 'IPv4'
  }
}

resource publicIPAddress 'Microsoft.Network/publicIPAddresses@2021-02-01' = {
  name: 'ace-static-pip'
  location: 'westeurope'
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
    publicIPPrefix: {
      id: publicIPPrefix.id
    }
  }
}
