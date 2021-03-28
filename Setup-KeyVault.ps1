<#
  The following script creates an application service principle to identify the application in Azure,
  thus allowing the application to interact with Azure, requesting config (secrets) from the KeyVault.
#>

$appName = "http://Bayou.Cloud.Integration.Test-app"
$keyVaultName = "wls-keyvault"

$rbacResults = (az ad sp create-for-rbac -n $appName --skip-assignment)

$rbacResults

$rbacResults = $rbacResults | ConvertFrom-Json

az keyvault set-policy --name $keyVaultName --spn $rbacResults.appId --secret-permissions backup delete get list set

# az keyvault show --name $keyVaultName

write-host "Assign the following environment variables, then restart Visual Studio:"
write-host " AZURE_CLIENT_ID = $($rbacResults.appId)"
write-host " AZURE_CLIENT_SECRET = $($rbacResults.password)"
write-host " AZURE_TENANT_ID = $($rbacResults.tenant)"
