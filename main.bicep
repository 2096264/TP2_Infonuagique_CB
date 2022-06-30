param location string = resourceGroup().location
param administratorLogin string = '123'
param administratorPassword string = ''
var nomsApps =[
  'CommandesAPI'
  'FavorisAPI'
  'FichiersAPI'
  'UtilisateursAPI'
  'VehiculesAPI'
  'ClientMVC'
]

var nomsBDs = [
  'Véhicules'
  'Utilisateurs'
  'Commandes'
]

var ContainerName = 'image'

module Apps 'modules/webapp.bicep' = [for nomApp in nomsApps: {
  name: nomApp
  params: {
    serviceplanName : 'SP-${nomApp}'
    location: location
    webAppName: nomApp
  }
}]

module BDs 'modules/sqlbd.bicep' = {
  name: nomBD
  params:{
    location: location
    administratorPassword:administratorPassword
    administratorLogin:administratorLogin
  }
}

module storageAccount 'modules/storageaccount.bicep' = {

  name: ContainerName
  params:{
    location: location
    storageName:ContainerName
  }
  
}
