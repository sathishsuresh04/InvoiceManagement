using FluentAssertions;
using InvoiceManagement.Application.Invoices.GetItemsReport;
using InvoiceManagement.Domain.Invoices;
using NSubstitute;

namespace InvoiceManagement.Application.Tests.Invoices.GetItemsReportQuery;

public class GetItemsReportQueryHandlerTests
{
    private readonly GetItemsReportQueryHandler _handler;
    private readonly IInvoiceRepository _invoiceRepository;

    public GetItemsReportQueryHandlerTests()
    {
        _invoiceRepository = Substitute.For<IInvoiceRepository>();
        _handler = new GetItemsReportQueryHandler(_invoiceRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_Report_When_DataExists()
    {
        // Arrange
        var query = new Application.Invoices.GetItemsReport.GetItemsReportQuery(
            DateTime.UtcNow.AddDays(-30),
            DateTime.UtcNow);
        var cancellationToken = new CancellationToken();
        var reportData =
            new Dictionary<string, long> {{"Item1", 10}, {"Item2", 20},} as IReadOnlyDictionary<string, long>;

        _invoiceRepository.GetItemsReportAsync(query.FromDate, query.ToDate)
            .Returns(Task.FromResult(reportData));

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        await _invoiceRepository.Received(1).GetItemsReportAsync(query.FromDate, query.ToDate);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(reportData);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_Report_When_NoDataExists()
    {
        // Arrange
        var query = new Application.Invoices.GetItemsReport.GetItemsReportQuery(
            DateTime.UtcNow.AddDays(-30),
            DateTime.UtcNow);
        var cancellationToken = new CancellationToken();
        var emptyReportData = new Dictionary<string, long>() as IReadOnlyDictionary<string, long>;

        _invoiceRepository.GetItemsReportAsync(query.FromDate, query.ToDate)
            .Returns(Task.FromResult(emptyReportData));

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        await _invoiceRepository.Received(1).GetItemsReportAsync(query.FromDate, query.ToDate);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
