resource kv1 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: 'kv1'
  location: 'westeurope'
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: '00000000-0000-0000-0000-000000000000'
  }
}

resource kv2 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: 'kv2'
  location: 'westeurope'
  properties: {
    sku: {
      family: 'A'
      name: 'premium'
    }
    tenantId: '00000000-0000-0000-0000-000000000000'
  }
}

resource kv3 'Microsoft.KeyVault/managedHSMs@2022-07-01' = {
  name: 'kv3'
  location: 'westeurope'
  sku: {
    name: 'Standard_B1'
    family: 'B'
  }
  properties: {
    tenantId: '11c43ee8-b9d3-4e51-b73f-bd9dda66e29c'
    initialAdminObjectIds: [
      '11c43ee8-b9d3-4e51-b73f-bd9dda66e29c'
    ]
  }
}

// resource kv4 'Microsoft.KeyVault/managedHSMs@2022-07-01' = {
//   name: 'kv4'
//   location: 'westeurope'
//   sku: {
//     name: 'Custom_B32'
//     family: 'B'
//   }
//   properties: {
//     tenantId: '11c43ee8-b9d3-4e51-b73f-bd9dda66e29c'
//     initialAdminObjectIds: [
//       '11c43ee8-b9d3-4e51-b73f-bd9dda66e29c'
//     ]
//   }
// }
