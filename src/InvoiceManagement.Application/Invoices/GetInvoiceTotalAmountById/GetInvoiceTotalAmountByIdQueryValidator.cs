using FluentValidation;
using InvoiceManagement.Application.Core.Errors;

namespace InvoiceManagement.Application.Invoices.GetInvoiceTotalAmountById;

public class GetInvoiceTotalAmountByIdQueryValidator : AbstractValidator<GetInvoiceTotalAmountByIdQuery>
{
    public GetInvoiceTotalAmountByIdQueryValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty()
            .WithMessage(ValidationErrors.GetInvoiceTotalAmountById.InvoiceIdIsRequired.Message);
    }
}
