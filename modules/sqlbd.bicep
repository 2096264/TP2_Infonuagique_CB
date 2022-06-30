param location string = resourceGroup().location
param dbServerName string = 'sqlbdServer'
param dbNameVehicules string = 'Véhicules'
param dbNameUtilisateurs string = 'Utilisateurs'
param dbNameCommandes string = 'Commandes'


resource SQLServer 'Microsoft.Sql/servers@2021-05-01-preview' = {
  name:'${dbServerName}-${uniqueString(resourceGroup().id)}'
  location:location

}

resource dataBaseVehicules 'Microsoft.Sql/servers/databases@2021-05-01-preview' = {
  parent:SQLServer
  name:dbNameVehicules
  location:location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}

resource dataBaseUtilisateurs 'Microsoft.Sql/servers/databases@2021-05-01-preview' = {
  parent:SQLServer
  name:dbNameUtilisateurs
  location:location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}

resource dataBaseCommandes 'Microsoft.Sql/servers/databases@2021-05-01-preview' = {
  parent:SQLServer
  name:dbNameCommandes
  location:location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}

resource firewallRules 'Microsoft.Sql/servers/firewallRules@2021-05-01-preview' = {
  parent: SQLServer
  name: 'AllowAzureIPs'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '255.255.255.255'
  }
}
