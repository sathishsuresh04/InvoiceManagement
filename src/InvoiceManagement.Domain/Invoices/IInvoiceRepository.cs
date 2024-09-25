namespace InvoiceManagement.Domain.Invoices;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id);
    void Insert(Invoice invoice);
    void Remove(Invoice invoice);
    Task<decimal?> GetTotalAmountAsync(Guid id);
    Task<decimal> GetTotalOfUnpaidAsync();
    Task<IReadOnlyDictionary<string, long>> GetItemsReportAsync(DateTime? from, DateTime? to);

    // New methods
    Task AddRangeAsync(IEnumerable<Invoice> invoices);
    Task<bool> AnyAsync();
}
