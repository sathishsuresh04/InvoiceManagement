using InvoiceManagement.Application.Abstractions.Messaging;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Domain.Primitives.Result;

namespace InvoiceManagement.Application.Invoices.GetTotalOfUnpaid;

public sealed class GetTotalOfUnpaidQueryHandler(IInvoiceRepository invoiceRepository)
    : IQueryHandler<GetTotalOfUnpaidQuery, Result<decimal>>
{
    public async Task<Result<decimal>> Handle(GetTotalOfUnpaidQuery request, CancellationToken cancellationToken)
    {
        var total = await invoiceRepository.GetTotalOfUnpaidAsync();
        return Result.Success(total);
    }
}
