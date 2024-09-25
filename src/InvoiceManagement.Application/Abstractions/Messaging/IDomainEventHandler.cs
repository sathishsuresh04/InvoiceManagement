using InvoiceManagement.Domain.Events;
using MediatR;

namespace InvoiceManagement.Application.Abstractions.Messaging;

/// <summary>
///     Represents a domain event handler interface.
/// </summary>
/// <typeparam name="TDomainEvent">The domain event type.</typeparam>
public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
where TDomainEvent : IDomainEvent
{
}
