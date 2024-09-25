using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;
using InvoiceManagement.Domain.Abstractions;
using InvoiceManagement.Domain.Errors;
using InvoiceManagement.Domain.Invoices.DomainEvents;
using InvoiceManagement.Domain.Primitives;
using InvoiceManagement.Domain.Primitives.Result;

namespace InvoiceManagement.Domain.Invoices;

[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public class Invoice : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
{
    public readonly List<InvoiceItem> _invoiceItems = [];

    private Invoice(string number, string seller, string buyer, string? description, DateTime issuedDate) : base(
        Guid.NewGuid())
    {
        Number = Guard.Against.NullOrWhiteSpace(number);
        Seller = Guard.Against.NullOrWhiteSpace(seller);
        Buyer = Guard.Against.NullOrWhiteSpace(buyer);
        Description = description;
        IssuedDate = Guard.Against.Default(issuedDate); // should not be null or default value
    }

    // A short description of an invoice (optional).
    public string? Description { get; private set; }

    // A number of an invoice e.g. 134/10/2018 (mandatory).
    public string Number { get; private set; }

    // An issuer of an invoice e.g. Metz-Anderson, 600 Hickman Street, Illinois (mandatory).
    public string Seller { get; private set; }

    // A buyer of a service or a product e.g. John Smith, 4285 Deercove Drive, Dallas (mandatory).
    public string Buyer { get; private set; }

    // // A date when an invoice was issued (mandatory).
    public DateTime IssuedDate { get; private set; }

    // A date when an invoice was paid (optional).
    public DateTime? AcceptanceDate { get; private set; }

    // A collection of invoice items for a given invoice (can be empty but is never null).

    public IReadOnlyList<InvoiceItem> InvoiceItems => _invoiceItems.AsReadOnly();

    public DateTime CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }
    public DateTime? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public static Invoice Create(string number, string seller, string buyer, string? description, DateTime issuedDate)
    {
        var invoice = new Invoice(number, seller, buyer, description, issuedDate);
        invoice.AddDomainEvent(new InvoiceCreatedEvent(invoice)); // publish domain event
        return invoice;
    }

    /// <summary>
    ///     Marks the invoice as accepted by setting the acceptance date to the specified UTC date and time.
    /// </summary>
    /// <param name="utcNow">The current date and time in UTC.</param>
    /// <returns>
    ///     A <see cref="Result" /> indicating the success or failure of the operation.
    ///     If the invoice is already paid, the result will be a failure with an appropriate error message.
    /// </returns>
    public  Result Accepted(DateTime utcNow)
    {
        if (AcceptanceDate == null || AcceptanceDate == default(DateTime))
            return Result.Failure(DomainErrors.Invoice.InvoiceIsAlreadyPaid);
        AcceptanceDate = utcNow;
        return Result.Success();
    }

    public Result AddInvoiceItem(InvoiceItem invoice)
    {
        //apply if there are any validation
        _invoiceItems.Add(invoice);
        return Result.Success();
    }
}
