resource bastion1 'Microsoft.Network/bastionHosts@2022-01-01' = {
  name: 'bastion1'
  location: 'westeurope'
  sku: {
    name: 'Basic'
  }
  properties: {

  }
}

resource bastion2 'Microsoft.Network/bastionHosts@2022-01-01' = {
  name: 'bastion2'
  location: 'westeurope'
  sku: {
    name: 'Standard'
  }
  properties: {

  }
}

resource bastion3 'Microsoft.Network/bastionHosts@2022-01-01' = {
  name: 'bastion3'
  location: 'westeurope'
  sku: {
    name: 'Standard'
  }
  properties: {
    scaleUnits: 1
  }
}
