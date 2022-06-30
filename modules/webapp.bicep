param webAppName string
param serviceplanName string
param location string
 
resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name:serviceplanName
  location:location
  sku:{
    name: (webAppName=='VehiculesAPI'  ? 'B1' : (webAppName == 'ClientMVC' ? 'S1' : 'F1'))
  }
}

resource webApps 'Microsoft.Web/sites@2021-02-01' = {
  name: '${webAppName}-${uniqueString(resourceGroup().id)}'
  location: location
  properties:{
    serverFarmId:appServicePlan.id
  }
}

resource webApplicationSlot 'Microsoft.Web/sites/slots@2021-02-01' = if (webAppName == 'ClientMVC') {
  name: '${webApps.name}/Staging'
  location: location
  properties: {
  serverFarmId: appServicePlan.id
 }
}
  

  



