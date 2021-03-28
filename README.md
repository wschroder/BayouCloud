# Bayou-Cloud
Demo app to experiment with Azure's API for .Net

Assumptions:
1. Existence of storage account.
2. Existence of key-vault 
3. Key vault name defined in Bayou.Cloud.IntegrationTest AppSettings (see "KeyVaultName")
4. Storage account connection string defined in KeyVault
5. Service principle defined for this test app, and given rights to the Key Vault.  (See Setup-KeyVault.ps1)
