using FluentAssertions;
using InvoiceManagement.Application.Invoices.GetTotalOfUnpaid;
using InvoiceManagement.Domain.Invoices;
using NSubstitute;

namespace InvoiceManagement.Application.Tests.Invoices.GetTotalOfUnpaid;

public class GetTotalOfUnpaidQueryHandlerTests
{
    private readonly GetTotalOfUnpaidQueryHandler _handler;
    private readonly IInvoiceRepository _invoiceRepository;

    public GetTotalOfUnpaidQueryHandlerTests()
    {
        _invoiceRepository = Substitute.For<IInvoiceRepository>();
        _handler = new GetTotalOfUnpaidQueryHandler(_invoiceRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_TotalOfUnpaid_When_DataExists()
    {
        // Arrange
        var query = new GetTotalOfUnpaidQuery();
        var cancellationToken = new CancellationToken();
        var totalUnpaid = 1000m;

        _invoiceRepository.GetTotalOfUnpaidAsync().Returns(Task.FromResult(totalUnpaid));

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        await _invoiceRepository.Received(1).GetTotalOfUnpaidAsync();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(totalUnpaid);
    }

    [Fact]
    public async Task Handle_Should_Return_Zero_When_NoUnpaidInvoicesExist()
    {
        // Arrange
        var query = new GetTotalOfUnpaidQuery();
        var cancellationToken = new CancellationToken();
        var totalUnpaid = 0m;

        _invoiceRepository.GetTotalOfUnpaidAsync().Returns(Task.FromResult(totalUnpaid));

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        await _invoiceRepository.Received(1).GetTotalOfUnpaidAsync();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(totalUnpaid);
    }
}
