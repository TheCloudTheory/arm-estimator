param rgLocation string = resourceGroup().location
param vnetName1 string
param vnetName2 string

metadata aceUsagePatterns = {
  Microsoft_Network_virtualNetworks_Inbound_data_transfer: '50'
  Microsoft_Network_virtualNetworks_Outbound_data_transfer: '150'
}

resource vnet1 'Microsoft.Network/virtualNetworks@2022-07-01' = {
  name: vnetName1
  location: rgLocation
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.1'
      ]
    }
    virtualNetworkPeerings: [
      {
        id: vnet2.id
      }
    ]
  }
}

resource vnet2 'Microsoft.Network/virtualNetworks@2022-07-01' = {
  name: vnetName2
  location: rgLocation
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.2'
      ]
    }
  }
}
