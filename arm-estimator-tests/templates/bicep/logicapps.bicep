resource la1 'Microsoft.Logic/workflows@2019-05-01' = {
  name: 'la1'
  location: 'westeurope'
  properties: {
  }
}

resource la2 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'logicapp'
  location: 'westeurope'
  sku: {
    name: 'WS1'
    tier: 'WorkflowStandard'
  }
  properties: {
  }
}

resource la22 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'logicapp22'
  location: 'westeurope'
  sku: {
    name: 'WS2'
    tier: 'WorkflowStandard'
  }
  properties: {
  }
}

resource la222 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'logicapp222'
  location: 'westeurope'
  sku: {
    name: 'WS3'
    tier: 'WorkflowStandard'
  }
  properties: {
  }
}

resource la3 'Microsoft.Logic/integrationAccounts@2019-05-01' = {
  name: 'la3'
  location: 'westeurope'
  sku: {
    name: 'Free'
  }
}

resource la4 'Microsoft.Logic/integrationAccounts@2019-05-01' = {
  name: 'la4'
  location: 'westeurope'
  sku: {
    name: 'Basic'
  }
}

resource la5 'Microsoft.Logic/integrationAccounts@2019-05-01' = {
  name: 'la5'
  location: 'westeurope'
  sku: {
    name: 'Standard'
  }
  properties: {
  }
}
