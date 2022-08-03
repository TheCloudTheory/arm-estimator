resource sa1 'Microsoft.StreamAnalytics/clusters@2020-03-01' = {
  name: 'sa1'
  location: 'westeurope'
  sku: {
   name: 'Default'
   capacity: 36 
  }
  properties: {
    
  }
}

resource sa2 'Microsoft.StreamAnalytics/streamingjobs@2021-10-01-preview' = {
  name: 'sa2'
  location: 'westeurope'
  properties: {
   sku: {
    name: 'Standard'
   } 
  }
}

resource sa3 'Microsoft.StreamAnalytics/streamingjobs@2021-10-01-preview' = {
  name: 'sa3'
  location: 'westeurope'
  properties: {
   sku: {
    name: 'Standard'
   } 
   transformation: {
    name: 'sa3'
    properties: {
      streamingUnits: 10
    }
   }
  }
}
