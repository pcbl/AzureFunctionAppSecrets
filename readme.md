# Azure Function APP, User Secrets and KeyVault

This sample have an Azure Function App that access User Secrets or KeyVault when the "KeyVaultName" is set.

```json
"KeyVaultName": "ABC"
```

If you provide the "KeyVaultName" setting (when locally on the local.settings.json file) within your application, it will look for the secrets within that vault. On this sample: ```https://ABC.vault.azure.net/```

Natuarally you would need to ensure that the function app has the [System Managed Identity](https://docs.microsoft.com/en-us/azure/app-service/overview-managed-identity?tabs=dotnet) set to *on*.

if you check the Terraform Folder, you will get a sample terraform template that:
1. Defines local variable, including a "tst" environment. 

2. **DO NOT FORGET** to update the "dev_id" variable (locals.tf) with your azure account object id (Should look like a GUID) by running the command bellow (azure cli required):
```
 az ad user show --id <USERNAME or PRINCIPAL NAME> --query objectId --out tsv
```

2. Creates an resource group named "gft-tests-secret-tst"

3. Creates a vault named "gft-secrets-test-tst"

4. Creates a secret named "the-secret" on the "gft-secrets-test-tst" vault

5. Adds the User ID (that you changed from 2) to the Vault Access Policy

Once you Publish the Azure Function, as long as it the [System Managed Identity](https://docs.microsoft.com/en-us/azure/app-service/overview-managed-identity?tabs=dotnet) set to *on*, you should be able to add it on the Vault Access Policy. 

To run locally, follow these steps to add the secret:
```
dotnet user-secrets init
dotnet user-secrets set "the-secret" "This is the local secret"
```

Some Useful links:
 - [Dependency Injection in Function Apps](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)
 - [Integrate Key Vault with Azure Functions](https://daniel-krzyczkowski.github.io/Integrate-Key-Vault-Secrets-With-Azure-Functions/)
 - [User Secrets on Function App](https://dev.to/cesarcodes/using-json-and-user-secrets-configuration-with-azure-functions-3f7g)
 

