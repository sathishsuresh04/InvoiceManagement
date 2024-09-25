using InvoiceManagement.Application.Abstractions.Messaging;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Domain.Primitives.Result;

namespace InvoiceManagement.Application.Invoices.GetItemsReport;

public sealed class GetItemsReportQueryHandler(IInvoiceRepository invoiceRepository)
    : IQueryHandler<GetItemsReportQuery, Result<IReadOnlyDictionary<string, long>>>
{
    /// <summary>
    ///     Handles the GetItemsReport query and returns the report as a result.
    /// </summary>
    /// <param name="request">The request containing the parameters for the report.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result containing a read-only dictionary with the items report.</returns>
    public async Task<Result<IReadOnlyDictionary<string, long>>> Handle(
        GetItemsReportQuery request,
        CancellationToken cancellationToken
    )
    {
        var total = await invoiceRepository.GetItemsReportAsync(request.FromDate, request.ToDate);
        return Result.Success(total);
    }
}
