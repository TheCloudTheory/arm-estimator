resource asp1 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'free'
  location: 'westeurope'
  sku: {
    name: 'F1'
  }
  properties: {
    
  }
}

resource asp2 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'shared'
  location: 'westeurope'
  sku: {
    name: 'D1'
  }
  properties: {
    
  }
}

resource asp3 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'basic1'
  location: 'westeurope'
  sku: {
    name: 'B1'
  }
  properties: {
    reserved: true
  }
}

resource asp4 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'basic2'
  location: 'westeurope'
  sku: {
    name: 'B2'
  }
  properties: {
    
  }
}

resource asp5 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'basic3'
  location: 'westeurope'
  sku: {
    name: 'B3'
  }
  properties: {
    
  }
}
