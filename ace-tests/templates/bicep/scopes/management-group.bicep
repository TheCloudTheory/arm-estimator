targetScope = 'managementGroup'

resource rbac 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('ace')
  properties: {
    principalId: '4917dcc9-9c0a-48b5-bc8f-c94e81df5823'
    roleDefinitionId: 'acdd72a7-3385-48ef-bd42-f606fba81ae7'
  }
}
