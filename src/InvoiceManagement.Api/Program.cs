using InvoiceManagement.Api.Common.Exception;
using InvoiceManagement.Api.Common.Swagger;
using InvoiceManagement.Api.Common.Versioning;
using InvoiceManagement.Application;
using InvoiceManagement.Application.Core.Extensions;
using InvoiceManagement.Infrastructure;
using InvoiceManagement.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;

namespace InvoiceManagement.Api;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseDefaultServiceProvider(
                (context, options) =>
                {
                    var isDevMode =
                        context.HostingEnvironment.IsDevelopment() ||
                        context.HostingEnvironment.IsEnvironment("test") ||
                        context.HostingEnvironment.IsStaging();

                    options.ValidateScopes = isDevMode;
                    options.ValidateOnBuild = isDevMode;
                });
            var openApiModel = builder.Configuration.GetOptions<OpenApiInfo>(nameof(OpenApiInfo));
            builder.Services.AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddPersistence(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>()
                .AddProblemDetails()
                .Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true)
                .AddCustomSwagger(openApiModel, typeof(Program).Assembly)
                .AddCustomVersioning();


            var app = builder.Build();
            app.UseExceptionHandler();
            var env = app.Environment;
            if (env.IsDevelopment()) await app.UseMigrationAsync(env);

            app.MapInvoiceEndpoints();
            app.UseCustomSwagger();

            await app.RunAsync();
        }
        //https://github.com/dotnet/runtime/issues/60600
        catch (Exception e) when (e is not OperationCanceledException && e.GetType().Name != "HostAbortedException")
        {
            Log.Fatal(e, "Program terminated unexpectedly!");
            throw;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}