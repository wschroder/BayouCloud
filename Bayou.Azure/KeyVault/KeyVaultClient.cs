using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;

namespace Bayou.Azure.KeyVault
{
    public static class KeyVaultManager
    {
        public static List<KeyValuePair<string, string>> DownloadKeyVaultValues(string keyVaultName)
        {
            // Note: Access to the keyvault relies on Role Based Access Control (RBAC), 
            // with this console application's identify configured as environment variables
            // (AZURE_CLIENT_ID, AZURE_CLIENT_SECRET, AZURE_TENANT_ID) that are picked up by
            // DefaultAzureCredentials.
            // Sett Setup-KeyVault.ps1 for details on setting this up.

            var kvUri = $"https://{keyVaultName}.vault.azure.net";

            var cred = new DefaultAzureCredential();
            // var cred = new VisualStudioCredential();

            var client = new SecretClient(new Uri(kvUri), cred);

            List<KeyValuePair<string, string>> secretsList = new List<KeyValuePair<string, string>>();

            IEnumerable<SecretProperties> secretProperties = client.GetPropertiesOfSecrets();
            foreach (SecretProperties secret in secretProperties)
            {
                KeyVaultSecret secretWithValue = client.GetSecret(secret.Name);
                var nameValuePair = new KeyValuePair<string, string>(secretWithValue.Name, secretWithValue.Value);
                secretsList.Add(nameValuePair);
            }
            return secretsList;
        }
    }
}
