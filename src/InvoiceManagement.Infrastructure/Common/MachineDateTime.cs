using InvoiceManagement.Application.Common;

namespace InvoiceManagement.Persistence.Common;

internal sealed class MachineDateTime : IDateTime
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}
