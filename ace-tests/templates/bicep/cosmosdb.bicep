resource cosmos1 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: 'cosmos1'
  location: 'westeurope'
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard' 
    locations: [
      {
        failoverPriority: 0
        isZoneRedundant: false
        locationName: 'westeurope'
      }
    ]
  }
}

resource database1 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2021-10-15' = {
  parent: cosmos1
  name: 'armestimator'
  properties: {
    resource: {
      id: 'armestimator'
    }
  }
}

resource container 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2021-10-15' = {
  parent: database1
  name: 'container1'
  properties: {
    resource: {
      id: 'container1'
    }
    options: {
      throughput: 400
    }
  }
}

resource cosmos2 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: 'cosmos2'
  location: 'westeurope'
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard' 
    locations: [
      {
        failoverPriority: 0
        isZoneRedundant: false
        locationName: 'westeurope'
      }
    ]
  }
}

resource database2 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2021-10-15' = {
  parent: cosmos2
  name: 'armestimator'
  properties: {
    resource: {
      id: 'armestimator'
    }
    options: {
      throughput: 1000
    }
  }
}

resource container2 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2021-10-15' = {
  parent: database2
  name: 'container2'
  properties: {
    resource: {
      id: 'container2'
    }
  }
}
