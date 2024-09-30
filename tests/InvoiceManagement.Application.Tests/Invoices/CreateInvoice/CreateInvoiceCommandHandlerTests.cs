using System.Reflection;
using FluentAssertions;
using InvoiceManagement.Application.Abstractions.Data;
using InvoiceManagement.Application.Invoices.CreateInvoice;
using InvoiceManagement.Domain.Invoices;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace InvoiceManagement.Application.Tests.Invoices.CreateInvoice;

public class CreateInvoiceCommandHandlerTests
{
    private readonly CreateInvoiceCommandHandler _handler;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvoiceCommandHandlerTests()
    {
        _invoiceRepository = Substitute.For<IInvoiceRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateInvoiceCommandHandler(_invoiceRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_Create_Invoice_And_Save_Changes()
    {
        // Arrange
        var command = new CreateInvoiceCommand("123", "Seller", "Buyer", "Description", DateTime.Today);
        var cancellationToken = new CancellationToken();

        // Mocking a successful insert
        _invoiceRepository.When(repo => repo.Insert(Arg.Any<Invoice>()))
            .Do(
                callInfo =>
                {
                    var invoice = callInfo.Arg<Invoice>();
                    // Simulate setting the Id internally by domain logic when inserted.
                    invoice.GetType()
                        .GetProperty(nameof(Invoice.Id), BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.SetValue(invoice, Guid.NewGuid());
                });

        _unitOfWork.SaveChangesAsync(cancellationToken)
            .Returns(Task.FromResult(1)); // Assuming SaveChangesAsync returns Task<int>

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        _invoiceRepository.Received(1)
            .Insert(
                Arg.Is<Invoice>(
                    i => i.Number == command.Number && i.Seller == command.Seller && i.Buyer == command.Buyer));
        await _unitOfWork.Received(1).SaveChangesAsync(cancellationToken);

        result.IsSuccess.Should().BeTrue();
        // Ensure invoice Id is set and returned as expected.
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_If_Repository_Throws_Exception()
    {
        // Arrange
        var command = new CreateInvoiceCommand("123", "Seller", "Buyer", "Description", DateTime.Today);
        var cancellationToken = new CancellationToken();

        _invoiceRepository.When(repo => repo.Insert(Arg.Any<Invoice>()))
            .Do(
                callInfo =>
                {
                    throw new Exception("Some repository exception");
                });

        // Act
        Func<Task> act = async () => await _handler.Handle(command, cancellationToken);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Some repository exception");
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_If_UnitOfWork_SaveChangesAsync_Throws_Exception()
    {
        // Arrange
        var command = new CreateInvoiceCommand("123", "Seller", "Buyer", "Description", DateTime.Today);
        var cancellationToken = new CancellationToken();

        _unitOfWork.SaveChangesAsync(cancellationToken).Throws(new Exception("Some unit of work exception"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, cancellationToken);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Some unit of work exception");
    }
}
