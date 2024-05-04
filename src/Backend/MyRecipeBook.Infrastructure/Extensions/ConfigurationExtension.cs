using Microsoft.Extensions.Configuration;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfigurationManager configuration)
    {
        return configuration.GetConnectionString("SqlServer")!;
    }
}

