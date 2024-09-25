using FluentValidation.TestHelper;
using InvoiceManagement.Application.Core.Errors;
using InvoiceManagement.Application.Invoices.GetInvoiceTotalAmountById;

namespace InvoiceManagement.Application.Tests.Invoices.GetInvoiceTotalAmountById;

public class GetInvoiceTotalAmountByIdQueryValidatorTests
{
    private readonly GetInvoiceTotalAmountByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_Should_HaveError_When_InvoiceId_IsEmpty()
    {
        // Arrange
        var query = new GetInvoiceTotalAmountByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.InvoiceId)
            .WithErrorMessage(ValidationErrors.GetInvoiceTotalAmountById.InvoiceIdIsRequired.Message);
    }

    [Fact]
    public void Validate_Should_NotHaveError_When_InvoiceId_IsNotEmpty()
    {
        // Arrange
        var query = new GetInvoiceTotalAmountByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.InvoiceId);
    }
}
