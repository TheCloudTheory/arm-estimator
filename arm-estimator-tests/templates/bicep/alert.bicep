resource existWinVm 'Microsoft.Compute/virtualMachines@2022-03-01' existing = {
  name: 'ace-vm-01'
}

resource existAG 'Microsoft.Insights/actionGroups@2021-09-01' existing = {
  name: 'ace-ag-01'
}

resource existLA 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' existing = {
  name: 'ace-la-v1'
}

resource cpuAlertDemo 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: 'cpuPercentageAbove80'
  location: 'global'
  tags: {
  }
  properties: {
    description: 'CPU is over 80%'
    severity: '3'
    enabled: true
    scopes: [
      existWinVm.id
    ]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT5M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: '1st criterion'
          metricName: 'Percentage CPU'
          dimensions: []
          operator: 'GreaterThan'
          threshold: '80'
          timeAggregation: 'Average'
        }
      ]
    }
    actions: [
      {
        actionGroupId: existAG.id
      }
    ]
  }
}

resource DisklogQueryAlert 'Microsoft.Insights/scheduledQueryRules@2018-04-16' = {
  name: 'Disk usage alert above 90 Perc'
  location: 'westeurope'
  properties: {
    description: 'This alert will be triggered when there is less than 10 Percentage low storage remaining.'
    enabled: 'true'
    source: {
      query: 'InsightsMetrics | where Origin == "vm.azm.ms" | where Computer == "acedevtest01" | where Namespace == "LogicalDisk" and Name == "FreeSpacePercentage" | summarize AggregatedValue = avg(Val) by bin(TimeGenerated, 15m), Computer'
      dataSourceId: existLA.id
      queryType: 'ResultCount'
    }
    schedule: {
      frequencyInMinutes: 15
      timeWindowInMinutes: 15
    }
    action: {
      'odata.type': 'Microsoft.WindowsAzure.Management.Monitoring.Alerts.Models.Microsoft.AppInsights.Nexus.DataContracts.Resources.ScheduledQueryRules.AlertingAction'
      severity: '4'
      aznsAction: {
        actionGroup: [resourceId('Microsoft.Insights/actionGroups', 'ace-ag-01')]
      }
      trigger: {
        thresholdOperator: 'LessThan'
        threshold: 10
        metricTrigger:{
          thresholdOperator: 'Equal'
          threshold: '1'
          metricColumn: 'Computer'
          metricTriggerType: 'Consecutive'
        }
      }
    }
  }
}
