resource redisCache01 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-01'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
  }
}

resource redisCache02 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-02'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 1
    }
  }
}

resource redisCache03 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-03'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 2
    }
  }
}

resource redisCache04 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-04'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 3
    }
  }
}

resource redisCache05 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-05'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 4
    }
  }
}

resource redisCache06 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-06'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 5
    }
  }
}

resource redisCache07 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-07'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 6
    }
  }
}

resource redisCache08 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-08'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard'
      family: 'C'
      capacity: 0
    }
  }
}

resource redisCache09 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-09'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard'
      family: 'C'
      capacity: 1
    }
  }
}

resource redisCache10 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-10'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard'
      family: 'C'
      capacity: 2
    }
  }
}

resource redisCache11 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-11'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard'
      family: 'C'
      capacity: 3
    }
  }
}

resource redisCache12 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-12'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard'
      family: 'C'
      capacity: 4
    }
  }
}

resource redisCache13 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-13'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard'
      family: 'C'
      capacity: 5
    }
  }
}

resource redisCache14 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-14'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Standard'
      family: 'C'
      capacity: 6
    }
  }
}

resource redisCache15 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-15'
  location: 'westeurope'
  properties: {
    shardCount : 1
    sku: {
      name: 'Premium'
      family: 'P'
      capacity: 1
    }
  }
}

resource redisCache16 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-16'
  location: 'westeurope'
  properties: {
    shardCount : 2
    sku: {
      name: 'Premium'
      family: 'P'
      capacity: 1
    }
  }
}

resource redisCache17 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-17'
  location: 'westeurope'
  properties: {
    replicasPerMaster : 2
    replicasPerPrimary : 2
    sku: {
      name: 'Premium'
      family: 'P'
      capacity: 1
    }
  }
}

resource redisCache18 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-18'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Premium'
      family: 'P'
      capacity: 2
    }
  }
}

resource redisCache19 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-19'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Premium'
      family: 'P'
      capacity: 3
    }
  }
}

resource redisCache20 'Microsoft.Cache/Redis@2021-06-01' = {
  name: 'ace-redis-20'
  location: 'westeurope'
  properties: {
    sku: {
      name: 'Premium'
      family: 'P'
      capacity: 4
    }
  }
}

