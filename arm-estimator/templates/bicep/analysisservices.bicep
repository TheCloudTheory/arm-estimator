resource as1 'Microsoft.AnalysisServices/servers@2017-08-01' = {
  name: 'as1'
  location: 'westeurope'
  sku: {
    name: 'B1'
    tier: 'Basic'
    capacity: 1
  }
  properties: {
  }
}

resource as2 'Microsoft.AnalysisServices/servers@2017-08-01' = {
  name: 'as2'
  location: 'westeurope'
  sku: {
    name: 'B2'
    tier: 'Basic'
    capacity: 1
  }
  properties: {
  }
}

resource as11 'Microsoft.AnalysisServices/servers@2017-08-01' = {
  name: 'as11'
  location: 'westeurope'
  sku: {
    name: 'B1'
    tier: 'Basic'
    capacity: 2
  }
  properties: {
  }
}

resource as3 'Microsoft.AnalysisServices/servers@2017-08-01' = {
  name: 'as3'
  location: 'westeurope'
  sku: {
    name: 'Developer'
    tier: 'Development'
    capacity: 1
  }
  properties: {
  }
}

resource as4 'Microsoft.AnalysisServices/servers@2017-08-01' = {
  name: 'as4'
  location: 'westeurope'
  sku: {
    name: 'S0'
    tier: 'Standard'
    capacity: 1
  }
  properties: {
  }
}

resource as5 'Microsoft.AnalysisServices/servers@2017-08-01' = {
  name: 'as5'
  location: 'westeurope'
  sku: {
    name: 'S1'
    tier: 'Standard'
    capacity: 1
  }
  properties: {
  }
}
