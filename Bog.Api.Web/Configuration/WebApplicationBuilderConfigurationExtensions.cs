using Azure.Identity;

namespace Bog.Api.Web.Configuration
{
    public static class WebApplicationBuilderConfigurationExtensions
    {
        public static WebApplicationBuilder WithApiConfiguration(this WebApplicationBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Configuration.AddEnvironmentVariables();

            var azKeyVaultId = builder.Configuration.GetValue<string>("AzKeyVault");

            if (string.IsNullOrWhiteSpace(azKeyVaultId))
            {
                return builder;
            }

            var keyVaultUrl = $"https://{azKeyVaultId}.vault.azure.net/";
            builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());

            return builder;
        }
    }
}