using Ardalis.GuardClauses;
using InvoiceManagement.Application.Core.Extensions;
using InvoiceManagement.Application.Data;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Persistence.Repositories;
using InvoiceManagement.Persistence.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace InvoiceManagement.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddValidateOptions<PostgresDbOptions>();
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<PostgresDbOptions>>().Value;
        Guard.Against.NullOrEmpty(options.ConnectionString);

        services.AddDbContext<InvoiceManagementDbContext>(
            (sp, dbContextOptionsBuilder) =>
            {
                var postgresDbOptions = sp.GetRequiredService<PostgresDbOptions>();
                Guard.Against.Null(postgresDbOptions, nameof(postgresDbOptions));


                dbContextOptionsBuilder.UseNpgsql(
                        postgresDbOptions.ConnectionString,
                        sqlOptions =>
                        {
                            if (!string.IsNullOrWhiteSpace(postgresDbOptions.DefaultSchema))
                            {
                                sqlOptions.MigrationsHistoryTable(
                                    "__efmigrationshistory",
                                    postgresDbOptions.DefaultSchema);
                            }

                            //  sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
                            sqlOptions.CommandTimeout(postgresDbOptions.CommandTimeoutInSeconds);
                        })
                    .UseSnakeCaseNamingConvention();
            });
        services.AddScoped<IDataSeeder, InvoiceDataSeeder>();
        services.AddScoped<IDbContext>(
            serviceProvider => serviceProvider.GetRequiredService<InvoiceManagementDbContext>());

        services.AddScoped<IUnitOfWork>(
            serviceProvider => serviceProvider.GetRequiredService<InvoiceManagementDbContext>());
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        return services;
    }

    public static async Task UseMigrationAsync(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        using var scope = app.ApplicationServices.CreateScope();
        try
        {
            await MigrateDatabaseAsync<InvoiceManagementDbContext>(app.ApplicationServices);
            if (env.IsDevelopment()) await SeedDataAsync(app.ApplicationServices);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred seeding the DB");
        }
    }

    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider)
    where TContext : DbContext, IDbContext
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
    }

    private static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seeders) await seeder.SeedAllAsync();
    }
}
