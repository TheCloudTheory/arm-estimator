param parSuffix string = utcNow('yyyyMMddHHmmss')
param parLocation string = 'australiaeast'
param parLocation2 string = 'brazilsouth'

resource vnet1 'Microsoft.Network/virtualNetworks@2023-09-01' = {
#disable-next-line use-stable-resource-identifiers
  name: 'vnet1-${parSuffix}'
  location: parLocation
  properties: {
    addressSpace: {
      addressPrefixes: [
        '192.168.0.0/16'
      ]
    }
  }
}

resource peering1 'Microsoft.Network/virtualNetworks/virtualNetworkPeerings@2023-09-01' = {
  name: 'vnet1-to-vnet2'
  parent: vnet1
  properties: {
    remoteVirtualNetwork: {
      id: vnet2.id
    }
  }
}

resource vnet2 'Microsoft.Network/virtualNetworks@2023-09-01' = {
  #disable-next-line use-stable-resource-identifiers
    name: 'vnet2-${parSuffix}'
    location: parLocation2
    properties: {
      addressSpace: {
        addressPrefixes: [
          '192.169.0.0/16'
        ]
      }
    }
  }

  resource peering2 'Microsoft.Network/virtualNetworks/virtualNetworkPeerings@2023-09-01' = {
    name: 'vnet2-to-vnet1'
    parent: vnet2
    properties: {
      remoteVirtualNetwork: {
        id: vnet1.id
      }
    }
  }
