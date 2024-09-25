using System.Reflection;
using FluentValidation;
using InvoiceManagement.Application.Core;

namespace InvoiceManagement.Application;

public static class DependencyInjection
{
    /// <summary>
    ///     Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));

                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });

        return services;
    }
}
