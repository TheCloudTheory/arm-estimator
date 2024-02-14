param parSuffix string = utcNow('yyyyMMddhhmmss')
param parLocation string = resourceGroup().location

resource app 'Microsoft.Web/staticSites@2023-01-01' = {
#disable-next-line use-stable-resource-identifiers
  name: 'static-site-${parSuffix}'
  location: parLocation
  sku: {
    name: 'Standard'
  }
  properties: {}
}
