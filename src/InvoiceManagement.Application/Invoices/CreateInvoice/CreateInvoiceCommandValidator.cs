using FluentValidation;
using InvoiceManagement.Application.Core.Errors;
using InvoiceManagement.Application.Core.Extensions;

namespace InvoiceManagement.Application.Invoices.CreateInvoice;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Number)
            .NotEmpty()
            .WithError(ValidationErrors.CreateInvoice.NumberIsRequired)
            .MaximumLength(50)
            .WithError(ValidationErrors.CreateInvoice.NumberMaxLengthExceeded);

        RuleFor(x => x.Seller)
            .NotEmpty()
            .WithError(ValidationErrors.CreateInvoice.SellerIsRequired)
            .MaximumLength(100)
            .WithError(ValidationErrors.CreateInvoice.SellerMaxLengthExceeded);

        RuleFor(x => x.Buyer)
            .NotEmpty()
            .WithError(ValidationErrors.CreateInvoice.BuyerIsRequired)
            .MaximumLength(100)
            .WithError(ValidationErrors.CreateInvoice.BuyerMaxLengthExceeded);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithError(ValidationErrors.CreateInvoice.DescriptionMaxLengthExceeded);

        RuleFor(x => x.IssuedDate)
            .LessThanOrEqualTo(DateTime.Today)
            .WithError(ValidationErrors.CreateInvoice.IssuedDateCannotBeInFuture);
    }
}
