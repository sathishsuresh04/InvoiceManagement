using InvoiceManagement.Application.Abstractions.Data;
using InvoiceManagement.Application.Abstractions.Messaging;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Domain.Primitives.Result;

namespace InvoiceManagement.Application.Invoices.CreateInvoice;

public sealed class CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateInvoiceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {

        var invoice = Invoice.Create(
            request.Number,
            request.Seller,
            request.Buyer,
            request.Description,
            request.IssuedDate);
        invoiceRepository.Insert(invoice);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(invoice.Id);

    }
}
