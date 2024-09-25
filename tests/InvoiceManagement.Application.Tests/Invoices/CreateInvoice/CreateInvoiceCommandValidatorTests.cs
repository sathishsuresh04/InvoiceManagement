using FluentValidation.TestHelper;
using InvoiceManagement.Application.Core.Errors;
using InvoiceManagement.Application.Invoices.CreateInvoice;

namespace InvoiceManagement.Application.Tests.CreateInvoice;

public class CreateInvoiceCommandValidatorTests
{
    private readonly CreateInvoiceCommandValidator _validator;

    public CreateInvoiceCommandValidatorTests()
    {
        _validator = new CreateInvoiceCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Number_Is_Empty()
    {
        // Arrange
        var command = new CreateInvoiceCommand(string.Empty, "Seller", "Buyer", "Description", DateTime.Today);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Number)
            .WithErrorMessage(ValidationErrors.CreateInvoice.NumberIsRequired.Message);
    }

    [Fact]
    public void Should_Have_Error_When_Number_Exceeds_Max_Length()
    {
        // Arrange
        var command = new CreateInvoiceCommand(new string('A', 51), "Seller", "Buyer", "Description", DateTime.Today);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Number)
            .WithErrorMessage(ValidationErrors.CreateInvoice.NumberMaxLengthExceeded.Message);
    }

    [Fact]
    public void Should_Have_Error_When_Seller_Is_Empty()
    {
        // Arrange
        var command = new CreateInvoiceCommand("Number", string.Empty, "Buyer", "Description", DateTime.Today);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Seller)
            .WithErrorMessage(ValidationErrors.CreateInvoice.SellerIsRequired.Message);
    }

    [Fact]
    public void Should_Have_Error_When_Seller_Exceeds_Max_Length()
    {
        // Arrange
        var command = new CreateInvoiceCommand("Number", new string('A', 101), "Buyer", "Description", DateTime.Today);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Seller)
            .WithErrorMessage(ValidationErrors.CreateInvoice.SellerMaxLengthExceeded.Message);
    }

    [Fact]
    public void Should_Have_Error_When_Buyer_Is_Empty()
    {
        // Arrange
        var command = new CreateInvoiceCommand("Number", "Seller", string.Empty, "Description", DateTime.Today);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Buyer)
            .WithErrorMessage(ValidationErrors.CreateInvoice.BuyerIsRequired.Message);
    }

    [Fact]
    public void Should_Have_Error_When_Buyer_Exceeds_Max_Length()
    {
        // Arrange
        var command = new CreateInvoiceCommand("Number", "Seller", new string('A', 101), "Description", DateTime.Today);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Buyer)
            .WithErrorMessage(ValidationErrors.CreateInvoice.BuyerMaxLengthExceeded.Message);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_Max_Length()
    {
        // Arrange
        var command = new CreateInvoiceCommand("Number", "Seller", "Buyer", new string('A', 501), DateTime.Today);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(ValidationErrors.CreateInvoice.DescriptionMaxLengthExceeded.Message);
    }

    [Fact]
    public void Should_Have_Error_When_IssuedDate_Is_In_Future()
    {
        // Arrange
        var command = new CreateInvoiceCommand("Number", "Seller", "Buyer", "Description", DateTime.Today.AddDays(1));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.IssuedDate)
            .WithErrorMessage(ValidationErrors.CreateInvoice.IssuedDateCannotBeInFuture.Message);
    }
}
