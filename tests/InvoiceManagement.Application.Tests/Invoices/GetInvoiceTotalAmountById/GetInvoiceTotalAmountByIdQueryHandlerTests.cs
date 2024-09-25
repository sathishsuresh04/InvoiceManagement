using FluentAssertions;
using InvoiceManagement.Application.Invoices.GetInvoiceTotalAmountById;
using InvoiceManagement.Domain.Invoices;
using NSubstitute;

namespace InvoiceManagement.Application.Tests.Invoices.GetInvoiceTotalAmountById;

public class GetInvoiceTotalAmountByIdQueryHandlerTests
{
    private readonly GetInvoiceTotalAmountByIdQueryHandler _handler;
    private readonly IInvoiceRepository _invoiceRepository;

    public GetInvoiceTotalAmountByIdQueryHandlerTests()
    {
        _invoiceRepository = Substitute.For<IInvoiceRepository>();
        _handler = new GetInvoiceTotalAmountByIdQueryHandler(_invoiceRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_TotalAmount_When_InvoiceExists()
    {
        // Arrange
        var query = new GetInvoiceTotalAmountByIdQuery(Guid.NewGuid());
        var cancellationToken = new CancellationToken();
        var totalAmount = 100m;

        _invoiceRepository.GetTotalAmountAsync(query.InvoiceId).Returns(Task.FromResult<decimal?>(totalAmount));

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        await _invoiceRepository.Received(1).GetTotalAmountAsync(query.InvoiceId);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(totalAmount);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_InvoiceDoesNotExist()
    {
        // Arrange
        var query = new GetInvoiceTotalAmountByIdQuery(Guid.NewGuid());
        var cancellationToken = new CancellationToken();

        _invoiceRepository.GetTotalAmountAsync(query.InvoiceId).Returns(Task.FromResult<decimal?>(null));

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        await _invoiceRepository.Received(1).GetTotalAmountAsync(query.InvoiceId);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }
}
