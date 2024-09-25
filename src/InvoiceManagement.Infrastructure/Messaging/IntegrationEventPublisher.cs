using InvoiceManagement.Application.Abstractions.Messaging;

namespace InvoiceManagement.Infrastructure.Messaging;

public class IntegrationEventPublisher : IIntegrationEventPublisher, IDisposable
{
    public void Dispose()
    {
    }

    public void Publish(IIntegrationEvent integrationEvent)
    {
    }
}
