resource eh1 'Microsoft.EventHub/namespaces@2022-01-01-preview' = {
  name: 'eh1'
  location: 'westeurope'
  sku: {
    name: 'Basic'
    capacity: 5
  }
  properties: {
    
  }
}

resource eh2 'Microsoft.EventHub/namespaces@2022-01-01-preview' = {
  name: 'eh2'
  location: 'westeurope'
  sku: {
    name: 'Standard'
    capacity: 10
  }
  properties: {
    
  }
}

resource eh2hub 'Microsoft.EventHub/namespaces/eventhubs@2022-01-01-preview' = {
  parent: eh2
  name: 'capture1'
  properties: {
    captureDescription: {
      enabled: true
    }
  }
}

resource eh3 'Microsoft.EventHub/namespaces@2022-01-01-preview' = {
  name: 'eh3'
  location: 'westeurope'
  sku: {
    name: 'Premium'
    capacity: 4
  }
  properties: {
    
  }
}

resource eh4 'Microsoft.EventHub/clusters@2022-01-01-preview' = {
  name: 'eh4'
  location: 'westeurope'
  sku: {
    name: 'Dedicated'
  }
  properties: {
    
  }
}
