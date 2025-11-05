using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace baitapabp.Data;

public class baitapabpDbContextFactory : IDesignTimeDbContextFactory<baitapabpDbContext>
{
    public baitapabpDbContext CreateDbContext(string[] args)
    {
        baitapabpGlobalFeatureConfigurator.Configure();
        baitapabpModuleExtensionConfigurator.Configure();

        baitapabpEfCoreEntityExtensionMappings.Configure();
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<baitapabpDbContext>()
            .UseMySQL(configuration.GetConnectionString("Default"));

        return new baitapabpDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}