resource eg1 'Microsoft.EventGrid/systemTopics@2022-06-15' = {
  name: 'eg1'
  location: 'westeurope'
  properties: {
  }
}

resource eg2 'Microsoft.EventGrid/topics@2022-06-15' = {
  name: 'eg2'
  location: 'westeurope'
  properties: {
  }
}

resource eg3 'Microsoft.EventGrid/eventSubscriptions@2022-06-15' = {
  name: 'eg3'
  scope: eg2
  properties: {

  }
}
