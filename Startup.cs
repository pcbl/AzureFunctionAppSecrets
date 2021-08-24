using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SecretsFunctionApp;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SecretsFunctionApp
{
    /// <summary>
    /// We use the Startup to support Dependency injection,
    /// so we can inject secrets over DotNet Secrets(in the absense of KeyVault)
    /// Or KeyVaul when the "KeyVaultName" is set!
    /// Some useful links:
    /// Sample to use UserSecrets:
    /// https://dev.to/cesarcodes/using-json-and-user-secrets-configuration-with-azure-functions-3f7g
    /// DI In Function APPS
    /// https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
    /// </summary>
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            //Building config so we can read the KeyVaultName parameter
            //DO NOT FORGET TO CONFIGURE THIS SETTING on your Function APP when deployed
            var builtConfig = builder.ConfigurationBuilder.Build();
            var keyVaultName = builtConfig["KeyVaultName"];

            //When a KeyVault is provided, we shall get secrets from there
            if (!string.IsNullOrEmpty(keyVaultName))
            {
                var secretClient = new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net/"),
                                                             new DefaultAzureCredential());
                
                // using Key Vault, either local dev or deployed
                builder.ConfigurationBuilder
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddAzureKeyVault(secretClient, new KeyVaultSecretManager())
                        .AddJsonFile("local.settings.json", true)
                        .Build();
            }
            else
            {
                //Otherwise local dev no Key Vault
                builder.ConfigurationBuilder
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                   .AddJsonFile("local.settings.json", true)
                   .Build();
            }
        }
    }
}