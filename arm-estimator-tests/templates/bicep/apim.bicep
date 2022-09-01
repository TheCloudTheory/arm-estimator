resource apim1 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: 'apimkamilm1'
  location: resourceGroup().location
  sku: {
    capacity: 0
    name: 'Consumption'
  }
  properties: {
    publisherEmail: 'johndoe@mail.com'
    publisherName: 'johndoe@mail.com'
  }
}

resource apim2 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: 'apimkamilm2'
  location: resourceGroup().location
  sku: {
    capacity: 1
    name: 'Basic'
  }
  properties: {
    publisherEmail: 'johndoe@mail.com'
    publisherName: 'johndoe@mail.com'
  }
}

resource apim22 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: 'apimkamilm22'
  location: resourceGroup().location
  sku: {
    capacity: 2
    name: 'Basic'
  }
  properties: {
    publisherEmail: 'johndoe@mail.com'
    publisherName: 'johndoe@mail.com'
  }
}

resource apim3 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: 'apimkamilm3'
  location: resourceGroup().location
  sku: {
    capacity: 1
    name: 'Developer'
  }
  properties: {
    publisherEmail: 'johndoe@mail.com'
    publisherName: 'johndoe@mail.com'
  }
}

resource apim4 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: 'apimkamilm4'
  location: resourceGroup().location
  sku: {
    capacity: 1
    name: 'Standard'
  }
  properties: {
    publisherEmail: 'johndoe@mail.com'
    publisherName: 'johndoe@mail.com'
  }
}

resource apim5 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: 'apimkamilm5'
  location: resourceGroup().location
  sku: {
    capacity: 1
    name: 'Premium'
  }
  properties: {
    publisherEmail: 'johndoe@mail.com'
    publisherName: 'johndoe@mail.com'
  }
}

resource gateway 'Microsoft.ApiManagement/service/gateways@2021-12-01-preview' = {
  parent: apim5
  name: 'apigateway'
  properties: {
    locationData: {
      name: 'location'
    }
  }
}

resource apim6 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: 'apimkamilm6'
  location: resourceGroup().location
  sku: {
    capacity: 2
    name: 'Premium'
  }
  properties: {
    publisherEmail: 'johndoe@mail.com'
    publisherName: 'johndoe@mail.com'
  }
}
