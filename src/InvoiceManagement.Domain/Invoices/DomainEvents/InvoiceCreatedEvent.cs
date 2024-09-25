using InvoiceManagement.Domain.Events;

namespace InvoiceManagement.Domain.Invoices.DomainEvents;

public class InvoiceCreatedEvent : IDomainEvent
{
    internal InvoiceCreatedEvent(Invoice invoice)
    {
        Invoice = invoice;
    }

    public Invoice Invoice { get; }
}
