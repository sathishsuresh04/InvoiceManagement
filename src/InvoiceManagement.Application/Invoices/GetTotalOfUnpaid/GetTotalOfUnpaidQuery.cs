using InvoiceManagement.Application.Abstractions.Messaging;
using InvoiceManagement.Domain.Primitives.Result;

namespace InvoiceManagement.Application.Invoices.GetTotalOfUnpaid;

public record GetTotalOfUnpaidQuery : IQuery<Result<decimal>>;
