using Humanizer;
using InvoiceManagement.Domain.Invoices;
using InvoiceManagement.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceManagement.Persistence.Configurations;

internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable(nameof(Invoice).Singularize().Underscore());
        builder.HasKey(invoice => invoice.Id);
        builder.Property(invoice => invoice.Number).HasColumnType(EfConstants.ColumnTypes.NormalText).IsRequired();
        builder.Property(invoice => invoice.Seller).HasColumnType(EfConstants.ColumnTypes.MediumText).IsRequired();
        builder.Property(invoice => invoice.Buyer).HasColumnType(EfConstants.ColumnTypes.MediumText).IsRequired();
        builder.Property(invoice => invoice.IssuedDate).IsRequired();
        builder.Property(invoice => invoice.Description)
            .HasColumnType(EfConstants.ColumnTypes.ExtraLongText)
            .IsRequired(false);
        builder.Property(invoice => invoice.CreatedOnUtc).IsRequired();

        builder.Property(invoice => invoice.ModifiedOnUtc);

        builder.Property(invoice => invoice.DeletedOnUtc);

        builder.Property(invoice => invoice.Deleted).HasDefaultValue(false);
        builder.HasQueryFilter(invoice => !invoice.Deleted); // default filter and always ignores Deleted invoices
    }
}
