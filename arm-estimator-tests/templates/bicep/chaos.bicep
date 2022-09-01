resource chaos 'Microsoft.Chaos/experiments@2021-09-15-preview' = {
  name: 'chaos'
  location: 'westeurope'
  properties: {
    selectors: [
      {
        type: 'List'
        targets: [
          {
            type: 'ChaosTarget'
            id: ''
          }
        ]
        id: ''
      }
    ]
    steps: [
      {
        name: ''
        branches: [
          {
            name: ''
            actions: [
              {
                type: 'delay'
                name: 'delay'
                duration: ''
              }
            ] 
          }
        ] 
      }
    ]
  }
}
