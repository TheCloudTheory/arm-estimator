targetScope = 'subscription'

param name string
param acrName string
param location string

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: name
  location: location
  properties: {
  }
}

module acr 'modules/acr.bicep' = {
  scope: rg
  name: 'deployment'
  params: {
    name: acrName
    location: location
  }
}
