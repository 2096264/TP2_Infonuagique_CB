param location string = resourceGroup().location
param dbServerName string = 'sqlbdServer'
param sku string 
var administratorLoginPassword = ''
var administratorLogin = ''
var dbNames = []



resource SQLServer 'Microsoft.Sql/servers@2021-05-01-preview' = {
  name:'${dbServerName}-${uniqueString(resourceGroup().id)}'
  location:location
  properties:{   
    administratorLoginPassword:administratorLoginPassword
    administratorLogin:administratorLogin
  }

}

resource dataBase 'Microsoft.Sql/servers/databases@2021-05-01-preview' = [for dbName in dbNames:{
  parent:SQLServer
  name:dbName
  location:location
  sku: {
    name: sku
    tier: sku
  }
}]



resource firewallRules 'Microsoft.Sql/servers/firewallRules@2021-05-01-preview' = {
  parent: SQLServer
  name: 'AllowAzureIPs'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '255.255.255.255'
  }
}
