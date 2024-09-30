using FluentAssertions;
using InvoiceManagement.Application.Abstractions.Common;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Persistence;
using InvoiceManagement.Persistence.Common;
using InvoiceManagement.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace InvoiceManagement.Repository.Tests;

public class InvoiceRepositoryTests
{
    private readonly InvoiceManagementDbContext _dbContext;
    private readonly InvoiceRepository _repository;

    public InvoiceRepositoryTests()
    {
        // Configure in-memory database context
        var options = new DbContextOptionsBuilder<InvoiceManagementDbContext>()
            .UseInMemoryDatabase("InvoiceDatabase")
            .Options;

        // Set up NSubstitute mocks
        var configuration = Substitute.For<IConfiguration>();
        configuration.GetSection(nameof(PostgresDbOptions))["DefaultSchema"].Returns("dbo");

        var dateTime = Substitute.For<IDateTime>();
        dateTime.UtcNow.Returns(DateTime.UtcNow);

        var mediator = Substitute.For<IMediator>();

        _dbContext = new InvoiceManagementDbContext(options, configuration, dateTime, mediator);

        // Ensure the database is clean before each test
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        _repository = new InvoiceRepository(_dbContext);
    }

    [Fact]
    public async Task GetTotalAmountAsync_Should_Return_TotalAmount_For_Invoice()
    {
        // Arrange
        var invoice = Invoice.Create("INV-001", "Seller", "Buyer", "Description", DateTime.UtcNow);
        invoice.AddInvoiceItem(InvoiceItem.Create("Item1", 2, 10.0m));
        invoice.AddInvoiceItem(InvoiceItem.Create("Item2", 3, 5.0m));

        _dbContext.Invoices.Add(invoice);
        await _dbContext.SaveChangesAsync();

        // Act
        var totalAmount = await _repository.GetTotalAmountAsync(invoice.Id);

        // Assert
        totalAmount.Should().Be(35.0m);
    }

    [Fact]
    public async Task GetTotalOfUnpaidAsync_Should_Return_Total_Of_Unpaid_Invoices()
    {
        // Arrange
        var invoice1 = Invoice.Create("INV-001", "Seller", "Buyer", "Description", DateTime.UtcNow);
        invoice1.AddInvoiceItem(InvoiceItem.Create("Item1", 2, 10.0m));
        invoice1.AddInvoiceItem(InvoiceItem.Create("Item2", 3, 5.0m));

        var invoice2 = Invoice.Create("INV-002", "Seller", "Buyer", "Description", DateTime.UtcNow.AddDays(-1));
        invoice2.Accepted(DateTime.UtcNow);
        invoice2.AddInvoiceItem(InvoiceItem.Create("Item3", 1, 7.0m));

        var invoice3 = Invoice.Create("INV-003", "Seller", "Buyer", "Description", DateTime.UtcNow);
        invoice3.AddInvoiceItem(InvoiceItem.Create("Item4", 1, 15.0m));

        _dbContext.Invoices.AddRange(invoice1, invoice2, invoice3);
        await _dbContext.SaveChangesAsync();

        // Act
        var totalUnpaid = await _repository.GetTotalOfUnpaidAsync();

        // Assert
        totalUnpaid.Should().Be(57.0m);
    }

    [Fact]
    public async Task GetItemsReportAsync_Should_Return_Item_Report()
    {
        // Arrange
        var from = DateTime.UtcNow.AddDays(-30);
        var to = DateTime.UtcNow;

        var invoice1 = Invoice.Create("INV-001", "Seller", "Buyer", "Description", DateTime.UtcNow.AddDays(-20));
        invoice1.AddInvoiceItem(InvoiceItem.Create("Item1", 1, 10.0m));
        invoice1.AddInvoiceItem(InvoiceItem.Create("Item2", 3, 5.0m));

        var invoice2 = Invoice.Create("INV-002", "Seller", "Buyer", "Description", DateTime.UtcNow.AddDays(-10));
        invoice2.AddInvoiceItem(InvoiceItem.Create("Item1", 2, 10.0m));

        var invoice3 = Invoice.Create("INV-003", "Seller", "Buyer", "Description", DateTime.UtcNow.AddDays(-40));
        invoice3.AddInvoiceItem(InvoiceItem.Create("Item1", 1, 10.0m));

        _dbContext.Invoices.AddRange(invoice1, invoice2, invoice3);
        await _dbContext.SaveChangesAsync();

        // Act
        var report = await _repository.GetItemsReportAsync(from, to);

        // Assert
        report.Should().ContainKey("Item1").WhoseValue.Should().Be(3);
        report.Should().ContainKey("Item2").WhoseValue.Should().Be(3);
    }
}