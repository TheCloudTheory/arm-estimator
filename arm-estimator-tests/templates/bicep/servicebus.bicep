resource sb1 'Microsoft.ServiceBus/namespaces@2021-11-01' = {
  name: 'sb1'
  location: resourceGroup().location
  sku: {
    name: 'Basic'
  }
  properties: {
    
  }
}

resource sb2 'Microsoft.ServiceBus/namespaces@2021-11-01' = {
  name: 'sb2'
  location: resourceGroup().location
  sku: {
    name: 'Standard'
  }
  properties: {
    
  }
}

resource sb3 'Microsoft.ServiceBus/namespaces@2021-11-01' = {
  name: 'sb3'
  location: resourceGroup().location
  sku: {
    name: 'Premium'
  }
  properties: {
    
  }
}
