resource vpn1 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn1'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Basic'
    }
  }
}

resource vpn2 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn2'
  location: 'westeurope'
  properties: {
    gatewayType: 'Vpn'
    vpnGatewayGeneration: 'Generation2'
    sku: {
      name: 'VpnGw1'
      tier: 'VpnGw1'
    }
  }
}

resource vpn3 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn3'
  location: 'westeurope'
  properties: {
    gatewayType: 'Vpn'
    sku: {
      name: 'VpnGw1AZ'
      tier: 'VpnGw1AZ'
    }
  }
}

resource vpn4 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn4'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'VpnGw2'
      tier: 'VpnGw2'
    }
  }
}

resource vpn5 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn5'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'VpnGw2AZ'
      tier: 'VpnGw2AZ'
    }
  }
}

resource vpn6 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn6'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'VpnGw3'
      tier: 'VpnGw3'
    }
  }
}

resource vpn7 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn7'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'VpnGw3AZ'
      tier: 'VpnGw3AZ'
    }
  }
}

resource vpn8 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn8'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'VpnGw4'
      tier: 'VpnGw4'
    }
  }
}

resource vpn9 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn9'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'VpnGw4AZ'
      tier: 'VpnGw4AZ'
    }
  }
}

resource vpn10 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn10'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'VpnGw5'
      tier: 'VpnGw5'
    }
  }
}

resource vpn11 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn11'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'VpnGw5AZ'
      tier: 'VpnGw5AZ'
    }
  }
}

resource vpn12 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn12'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard'
      tier: 'Standard'
    }
  }
}

resource vpn13 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn13'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'ErGw1AZ'
      tier: 'ErGw1AZ'
    }
  }
}

resource vpn14 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn14'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'ErGw2AZ'
      tier: 'ErGw2AZ'
    }
  }
}

resource vpn15 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn15'
  location: 'westeurope'
  properties: {
    vpnGatewayGeneration: 'Generation2'
    sku: {
      name: 'ErGw3AZ'
      tier: 'ErGw3AZ'
    }
  }
}

resource vpn16 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn16'
  location: 'westeurope'
  properties: {
    vpnGatewayGeneration: 'Generation2'
    sku: {
      name: 'HighPerformance'
      tier: 'HighPerformance'
    }
  }
}

resource vpn17 'Microsoft.Network/virtualNetworkGateways@2022-01-01' = {
  name: 'vpn17'
  location: 'westeurope'
  properties: {
    vpnGatewayGeneration: 'Generation2'
    sku: {
      name: 'UltraPerformance'
      tier: 'UltraPerformance'
    }
  }
}
