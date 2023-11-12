resource ai 'Microsoft.Insights/components@2020-02-02' = {
  name: 'aiestimator'
  location: 'westeurope'
  kind: 'other'
  properties: {
    Application_Type: 'other'
  }
}
