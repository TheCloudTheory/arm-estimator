resource ac 'Microsoft.AppConfiguration/configurationStores@2022-05-01' = {
  name: 'appconfiguration'
  location: 'westeurope'
  sku: {
    name: 'Free'
  }
}

resource ac2 'Microsoft.AppConfiguration/configurationStores@2022-05-01' = {
  name: 'appconfiguration2'
  location: 'westeurope'
  sku: {
    name: 'Standard'
  }
}
