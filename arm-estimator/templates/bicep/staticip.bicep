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

resource publicIPPrefixGlobal 'Microsoft.Network/publicIPPrefixes@2021-02-01' = {
  name: 'ace-pip-global-prefix'
  location: 'westeurope'
  sku: {
    name: 'Standard'
    tier: 'Global'
  }
  properties: {
    prefixLength: '29'
    publicIPAddressVersion: 'IPv4'
  }
}

resource publicIPAddressGlobal 'Microsoft.Network/publicIPAddresses@2021-02-01' = {
  name: 'ace-static-global-pip'
  location: 'westeurope'
  sku: {
    name: 'Standard'
    tier: 'Global'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
    publicIPPrefix: {
      id: publicIPPrefixGlobal.id
    }
  }
}

resource publicIPPrefixBasic 'Microsoft.Network/publicIPPrefixes@2021-02-01' = {
  name: 'ace-pip-basic-prefix'
  location: 'westeurope'
  sku: {
    name: 'Basic'
    tier: 'Regional'
  }
  properties: {
    prefixLength: '29'
    publicIPAddressVersion: 'IPv4'
  }
}

resource publicIPAddressBasic 'Microsoft.Network/publicIPAddresses@2021-02-01' = {
  name: 'ace-static-basic-pip'
  location: 'westeurope'
  sku: {
    name: 'Basic'
    tier: 'Regional'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
    publicIPPrefix: {
      id: publicIPPrefixBasic.id
    }
  }
}
