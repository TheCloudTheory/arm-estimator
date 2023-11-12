resource bs1 'Microsoft.BotService/botServices@2021-05-01-preview' = {
  name: 'bs1'
  location: 'westeurope'
  sku: {
    name: 'F0'
  }
  properties: {
    endpoint: ''
    displayName: 'some-name'
    msaAppId: ''
  }
}

resource bs2 'Microsoft.BotService/botServices@2021-05-01-preview' = {
  name: 'bs2'
  location: 'westeurope'
  sku: {
    name: 'S1'
  }
  properties: {
    endpoint: ''
    displayName: 'some-name'
    msaAppId: ''
  }
}

resource hb1 'Microsoft.HealthBot/healthBots@2021-08-24' = {
  name: 'hb1'
  location: 'westeurope'
  sku: {
    name: 'F0'
  }
  properties: {
  }
}

resource hb2 'Microsoft.HealthBot/healthBots@2021-08-24' = {
  name: 'hb2'
  location: 'westeurope'
  sku: {
    name: 'S1'
  }
  properties: {
  }
}
