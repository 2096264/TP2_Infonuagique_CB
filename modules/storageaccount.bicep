param location string = resourceGroup().location
param storageName string

var containerName = 'image'
 

resource storageAccount 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-09-01' = {
  
  name:'${storageName}${containerName}'
  location: location
  properties: { 
    publicAccess: 'Container'
  }
}
  
  
