using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InvoiceManagement.Persistence.Extensions;

internal static class ModelBuilderExtensions
{
    private static readonly ValueConverter<DateTime, DateTime> _utcValueConverter =
        new(outside => outside, inside => DateTime.SpecifyKind(inside, DateTimeKind.Utc));

    /// <summary>
    ///     Applies the UTC date-time converter to all of the properties that are <see cref="DateTime" /> and end with Utc.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    internal static void ApplyUtcDateTimeConverter(this ModelBuilder modelBuilder)
    {
        foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
        {
            var dateTimeUtcProperties = mutableEntityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) && p.Name.EndsWith("Utc", StringComparison.Ordinal));

            foreach (var mutableProperty in dateTimeUtcProperties)
                mutableProperty.SetValueConverter(_utcValueConverter);
        }
    }
}
