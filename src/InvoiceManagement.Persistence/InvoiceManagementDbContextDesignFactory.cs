using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace InvoiceManagement.Persistence;

public class InvoiceManagementDbContextDesignFactory : IDesignTimeDbContextFactory<InvoiceManagementDbContext>
{
    public InvoiceManagementDbContext CreateDbContext(string[] args)
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "test";

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", true) // it is optional
            .AddEnvironmentVariables();

        var configuration = configurationBuilder.Build();
        var connectionString = configuration["PostgresDbOptions:ConnectionString"];
        var defaultSchema = configuration["PostgresDbOptions:DefaultSchema"];


        var builder = new DbContextOptionsBuilder<InvoiceManagementDbContext>();

        builder.UseNpgsql(
                connectionString,
                sqlOptions =>
                {
                    if (!string.IsNullOrWhiteSpace(defaultSchema))
                    {
                        sqlOptions.MigrationsHistoryTable(
                            "__efmigrationshistory",
                            defaultSchema);
                    }
                })
            .UseSnakeCaseNamingConvention();

        return new InvoiceManagementDbContext(builder.Options, configuration, null!, null!);
    }
}
