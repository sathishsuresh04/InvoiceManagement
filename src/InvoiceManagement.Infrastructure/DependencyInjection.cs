using InvoiceManagement.Application.Abstractions.Common;
using InvoiceManagement.Application.Abstractions.Messaging;
using InvoiceManagement.Application.Core.Extensions;
using InvoiceManagement.Infrastructure.Common;
using InvoiceManagement.Infrastructure.Emails;
using InvoiceManagement.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;

namespace InvoiceManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //TODO email service and JWT issuer..etc can be configured here
        services.AddValidateOptions<EmailOptions>();
        services.AddValidateOptions<MessageBrokerOptions>();
        services.AddTransient<IDateTime, MachineDateTime>();
        services.AddSingleton<IIntegrationEventPublisher, IntegrationEventPublisher>();
        return services;
    }
}
