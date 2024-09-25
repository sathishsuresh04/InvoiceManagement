using InvoiceManagement.Application.Data;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagement.Persistence.Repositories;

public sealed class InvoiceRepository(IDbContext dbContext)
    : GenericRepository<Invoice>(dbContext), IInvoiceRepository

{
    public async Task<decimal?> GetTotalAmountAsync(Guid id)
    {
        var totalAmount = await DbContext.Set<Invoice>()
                              .Where(x => x.Id == id)
                              .SelectMany(x => x.InvoiceItems)
                              .SumAsync(item => (decimal?)(item.Price * item.Count));

        return totalAmount ?? 0m;
    }

    /// <inheritdoc />
    public async Task<decimal> GetTotalOfUnpaidAsync()
    {
        var totalUnpaid = await DbContext.Set<Invoice>()
                              .Where(i => !i.AcceptanceDate.HasValue)
                              .SelectMany(i => i.InvoiceItems)
                              .SumAsync(item => item.Price * item.Count);

        return totalUnpaid;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyDictionary<string, long>> GetItemsReportAsync(DateTime? from, DateTime? to)
    {
        var query = DbContext.Set<Invoice>().AsQueryable();

        if (from.HasValue) query = query.Where(i => i.IssuedDate >= from.Value);

        if (to.HasValue) query = query.Where(i => i.IssuedDate <= to.Value);

        var result = await query
                         .SelectMany(i => i.InvoiceItems)
                         .GroupBy(item => item.Name)
                         .Select(g => new {ItemName = g.Key, TotalCount = g.Sum(item => item.Count),})
                         .ToDictionaryAsync(x => x.ItemName, x => x.TotalCount);

        return result;
    }

    public Task<bool> AnyAsync()
    {
        return base.AnyAsync(new InvoiceSpecification());
    }
}
