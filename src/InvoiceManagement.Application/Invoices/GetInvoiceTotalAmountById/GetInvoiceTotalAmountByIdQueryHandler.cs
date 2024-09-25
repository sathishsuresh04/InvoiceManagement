using InvoiceManagement.Application.Abstractions.Messaging;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Domain.Primitives.Result;

namespace InvoiceManagement.Application.Invoices.GetInvoiceTotalAmountById;

public sealed class GetInvoiceTotalAmountByIdQueryHandler(IInvoiceRepository invoiceRepository)
    : IQueryHandler<GetInvoiceTotalAmountByIdQuery, Result<decimal?>>
{
    public async Task<Result<decimal?>> Handle(
        GetInvoiceTotalAmountByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var total = await invoiceRepository.GetTotalAmountAsync(request.InvoiceId);
        return Result.Success(total);
    }
}
