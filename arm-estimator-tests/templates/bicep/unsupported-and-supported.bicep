resource acr 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: 'metadataacr'
  location: resourceGroup().location
  sku: {
    name: 'Basic'
  }
}

resource pipeline 'Microsoft.DevOps/pipelines@2020-07-13-preview' = {
  name: 'pipeline'
  properties: {
    bootstrapConfiguration: {
      template: {
        id: 'id'
      }
    }
    organization: {
      name: 'thecloudtheory'
    }
    pipelineType: 'azurePipeline'
    project: {
      name: 'arm-estimator'
    }
  }
}
