using InvoiceManagement.Application.Abstractions.Common;

namespace InvoiceManagement.Infrastructure.Common;

internal sealed class MachineDateTime : IDateTime
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}
