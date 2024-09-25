using Bogus;
using InvoiceManagement.Application.Data;
using InvoiceManagement.Domain.Invoices;

namespace InvoiceManagement.Persistence.Seed;

public class InvoiceDataSeeder(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
    : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        if (await invoiceRepository.AnyAsync()) return;

        var invoices = new InvoiceSeedFaker().Generate(50);
        await invoiceRepository.AddRangeAsync(invoices);

        await unitOfWork.SaveChangesAsync();
    }

    private sealed class InvoiceSeedFaker : Faker<Invoice>
    {
        public InvoiceSeedFaker()
        {
            RuleFor(x => x.Number, f => f.Commerce.Ean8())
                .RuleFor(x => x.Seller, f => f.Company.CompanyName())
                .RuleFor(x => x.Buyer, f => f.Person.FullName)
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.IssuedDate, f => f.Date.Past())
                .RuleFor(x => x.AcceptanceDate, f => f.Random.Bool() ? f.Date.Past() : null)
                .RuleFor(x => x._invoiceItems, f => new InvoiceItemSeedFaker().Generate(10).ToList())
                .CustomInstantiator(
                    f => Invoice.Create(
                        f.Commerce.Ean8(),
                        f.Company.CompanyName(),
                        f.Person.FullName,
                        f.Lorem.Sentence(),
                        f.Date.Past()));
        }
    }

    private sealed class InvoiceItemSeedFaker : Faker<InvoiceItem>
    {
        public InvoiceItemSeedFaker()
        {
            RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Count, f => (uint)f.Random.Number(1, 100))
                .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
                .RuleFor(x => x.CreatedOnUtc, f => f.Date.Past())
                .RuleFor(x => x.ModifiedOnUtc, f => f.Date.Past())
                .CustomInstantiator(
                    f => InvoiceItem.Create(
                        f.Commerce.ProductName(),
                        (uint)f.Random.Number(1, 100),
                        f.Random.Decimal(1, 1000)));
        }
    }
}
