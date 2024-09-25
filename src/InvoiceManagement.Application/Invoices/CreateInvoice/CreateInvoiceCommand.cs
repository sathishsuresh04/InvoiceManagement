using InvoiceManagement.Application.Abstractions.Messaging;
using InvoiceManagement.Domain.Primitives.Result;

namespace InvoiceManagement.Application.Invoices.CreateInvoice;

public record CreateInvoiceCommand(string Number, string Seller, string Buyer, string? Description, DateTime IssuedDate)
    : ICommand<Result<Guid>>;
