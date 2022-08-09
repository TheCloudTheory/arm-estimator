resource signal1 'Microsoft.SignalRService/signalR@2022-02-01' = {
  name: 'signalr1arm'
  location: 'westeurope'
  sku: {
    name: 'Free_F1'
  }
  properties: {
  }
}

resource signal2 'Microsoft.SignalRService/signalR@2022-02-01' = {
  name: 'signalr2arm'
  location: 'westeurope'
  sku: {
    name: 'Standard_S1'
  }
  properties: {
  }
}

resource signal22 'Microsoft.SignalRService/signalR@2022-02-01' = {
  name: 'signalr22arm'
  location: 'westeurope'
  sku: {
    name: 'Standard_S1'
    capacity: 2
    tier: 'Standard'
  }
  properties: {
  }
}

resource signal3 'Microsoft.SignalRService/signalR@2022-02-01' = {
  name: 'signalr3arm'
  location: 'westeurope'
  sku: {
    name: 'Premium_P1'
  }
  properties: {
  }
}
