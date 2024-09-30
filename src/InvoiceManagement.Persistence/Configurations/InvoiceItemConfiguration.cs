using Humanizer;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceManagement.Persistence.Configurations;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable(nameof(InvoiceItem).Singularize().Underscore());
        builder.HasKey(invoiceItem => invoiceItem.Id);
        builder.Property(invoiceItem => invoiceItem.Name)
            .HasColumnType(EfConstants.ColumnTypes.NormalText)
            .IsRequired();
        builder.Property(invoiceItem => invoiceItem.Count).IsRequired();
        builder.Property(invoiceItem => invoiceItem.Price)
            .HasColumnType(EfConstants.ColumnTypes.DecimalTenTwo)
            .IsRequired();
        builder.Property(invoiceItem => invoiceItem.CreatedOnUtc).IsRequired();

        builder.Property(invoiceItem => invoiceItem.ModifiedOnUtc);
        builder.HasOne<Invoice>()
            .WithMany(invoice => invoice.InvoiceItems)
            .HasForeignKey(invoiceItem => invoiceItem.InvoiceId);
    }
}
