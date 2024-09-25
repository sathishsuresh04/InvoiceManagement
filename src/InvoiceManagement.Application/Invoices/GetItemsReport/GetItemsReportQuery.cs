using InvoiceManagement.Application.Abstractions.Messaging;
using InvoiceManagement.Domain.Primitives.Result;

namespace InvoiceManagement.Application.Invoices.GetItemsReport;

public record GetItemsReportQuery(DateTime? FromDate, DateTime? ToDate)
    : IQuery<Result<IReadOnlyDictionary<string, long>>>;
