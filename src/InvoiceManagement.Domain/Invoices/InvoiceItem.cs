using Ardalis.GuardClauses;
using InvoiceManagement.Domain.Abstractions;
using InvoiceManagement.Domain.Primitives;

namespace InvoiceManagement.Domain.Invoices;

public class InvoiceItem : Entity, IAuditableEntity
{
    private InvoiceItem(string name, uint count, decimal price) : base(Guid.NewGuid())
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Count = count;
        Price = Guard.Against.NegativeOrZero(price);
    }

    // A name of an item e.g. eggs.
    public string Name { get; private set; }

    // A number of bought items e.g. 10.
    public uint Count { get; private set; } // Changed to uint to ensure it's a non-negative number.

    // A price of an item e.g. 20.5.
    public decimal Price { get; private set; }

    /// <summary>
    ///     Gets the Invoice identifier.
    /// </summary>
    public Guid InvoiceId { get; }


    public DateTime CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }

    public static InvoiceItem Create(string name, uint count, decimal price)
    {
        var invoiceItem = new InvoiceItem(name, count, price);
        return invoiceItem;
    }
}
