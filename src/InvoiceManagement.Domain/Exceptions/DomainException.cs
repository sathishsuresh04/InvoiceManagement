using InvoiceManagement.Domain.Primitives;

namespace InvoiceManagement.Domain.Exceptions;

public class DomainException : Exception
{
    /// <summary>
    ///     Represents an exception specific to domain logic errors.
    /// </summary>
    public DomainException(Error error)
        : base(error.Message)
    {
        Error = error;
    }

    /// <summary>
    ///     Gets the <see cref="Error" /> representing the domain-specific error encountered.
    /// </summary>
    public Error Error { get; }
}
