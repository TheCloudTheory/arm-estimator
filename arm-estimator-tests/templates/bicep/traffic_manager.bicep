resource existPIP 'Microsoft.Network/publicIPAddresses@2020-06-01' existing = {
  name: 'ace-pip-01'
}

resource ExternalEndpoint 'Microsoft.Network/trafficmanagerprofiles@2018-08-01' = {
  name: 'ACEExternalEndpoint'
  location: 'global'
  properties: {
    profileStatus: 'Enabled'
    trafficRoutingMethod: 'Priority'
    trafficViewEnrollmentStatus: 'Enabled'
    dnsConfig: {
      relativeName: 'ace-dns-01'
      ttl: 30
    }
    monitorConfig: {
      protocol: 'HTTP'
      port: 80
      path: '/'
    }
    endpoints: [
      {
        type: 'Microsoft.Network/TrafficManagerProfiles/azureEndpoints'
        name: 'AzureEndpoint'
        properties: {
          targetResourceId : existPIP.id
          endpointStatus: 'Enabled'
          endpointLocation: 'westeurope'
          priority: 1
        }
      }
      {
        type: 'Microsoft.Network/TrafficManagerProfiles/ExternalEndpoints'
        name: 'ExternalEndpoint'
        properties: {
          target: 'www.microsoft.com'
          endpointStatus: 'Enabled'
          endpointLocation: 'northeurope'
          priority: 2
        }
      }
    ]
  }
}
