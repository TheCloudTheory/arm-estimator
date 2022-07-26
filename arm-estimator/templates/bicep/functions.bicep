resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: 'armestimatorfunction'
  location: 'westeurope'
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}

resource af 'Microsoft.Web/sites@2022-03-01' = {
  name: 'armestimatorfunction'
  location: 'westeurope'
  kind: 'functionapp'
  properties: {
    serverFarmId: hostingPlan.id
  }
}

resource hostingPlanPremium 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: 'armestimatorfunctionpremium'
  location: 'westeurope'
  sku: {
    name: 'EP1'
    tier: 'ElasticPremium'
  }
  properties: {}
}
