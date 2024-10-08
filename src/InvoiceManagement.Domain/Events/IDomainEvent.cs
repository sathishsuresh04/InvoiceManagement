using MediatR;

namespace InvoiceManagement.Domain.Events;

/// <summary>
///     Represents the interface for an event that is raised within the domain.
/// </summary>
public interface IDomainEvent : INotification
{
}
