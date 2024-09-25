using InvoiceManagement.Application.Abstractions.Messaging;
using InvoiceManagement.Domain.Primitives.Result;

namespace InvoiceManagement.Application.Invoices.GetInvoiceTotalAmountById;

public record GetInvoiceTotalAmountByIdQuery(Guid InvoiceId) : IQuery<Result<decimal?>>;
