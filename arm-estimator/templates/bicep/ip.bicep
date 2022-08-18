resource pip06 'Microsoft.Network/publicIPAddresses@2020-06-01' = {
  name: 'ace-pip-06'
  location: 'westeurope'
  properties: {
    publicIPAllocationMethod: 'Dynamic'
  }
}
