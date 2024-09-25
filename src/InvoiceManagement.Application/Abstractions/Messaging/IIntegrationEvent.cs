using MediatR;

namespace InvoiceManagement.Application.Abstractions.Messaging;

/// <summary>
///     Represents the marker interface for an integration event.
/// </summary>
public interface IIntegrationEvent : INotification
{
}
