resource ts1 'Microsoft.TimeSeriesInsights/environments@2021-06-30-preview' = {
  name: 'ts1'
  location: 'westeurope'
  sku: {
    capacity: 1
    name: 'S1'
  }
  kind: 'Gen2'
  properties: {
    storageConfiguration: {
      managementKey: ''
      accountName: ''
    }
    timeSeriesIdProperties: [
      
    ]
  }
}

resource ts11 'Microsoft.TimeSeriesInsights/environments@2021-06-30-preview' = {
  name: 'ts11'
  location: 'westeurope'
  sku: {
    capacity: 2
    name: 'S1'
  }
  kind: 'Gen2'
  properties: {
    storageConfiguration: {
      managementKey: ''
      accountName: ''
    }
    timeSeriesIdProperties: [
      
    ]
  }
}

resource ts2 'Microsoft.TimeSeriesInsights/environments@2021-06-30-preview' = {
  name: 'ts2'
  location: 'westeurope'
  sku: {
    capacity: 1
    name: 'S2'
  }
  kind: 'Gen2'
  properties: {
    storageConfiguration: {
      managementKey: ''
      accountName: ''
    }
    timeSeriesIdProperties: [
      
    ]
  }
}
