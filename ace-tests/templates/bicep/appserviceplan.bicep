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

resource asp6 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'asp6'
  location: 'eastus2'
  sku: {
    name: 'P1V2'
  }
  properties: {
  }
}

resource asp7 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'asp7'
  location: 'northeurope'
  sku: {
    name: 'P2V3'
  }
  properties: {
  }
}

resource asp8 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'asp8'
  location: 'westeurope'
  sku: {
    name: 'B1'
  }
  properties: {
    reserved: true
  }
}

resource asp9 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'asp9'
  location: 'westeurope'
  sku: {
    name: 'P1V2'
  }
  properties: {
    
  }
}

resource asp10 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'asp10'
  location: 'uksouth'
  sku: {
    name: 'S2'
  }
  properties: {
    reserved: true
  }
}

resource basicwithcapacity 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'basic-with-capacity'
  location: 'westeurope'
  sku: {
    name: 'B1'
    capacity: 3
  }
  properties: {
    reserved: true
  }
}
