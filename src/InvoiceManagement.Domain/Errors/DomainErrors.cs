using InvoiceManagement.Domain.Primitives;

namespace InvoiceManagement.Domain.Errors;

public static class DomainErrors
{
    /// <summary>
    ///     Contains the attendee errors.
    /// </summary>
    public static class Invoice
    {
        public static Error NotFound => Error.NotFound(
            "Invoice.NotFound",
            "The Invoice with the specified identifier was not found.");

        public static Error InvoiceIsAlreadyPaid => Error.Validation(
            "Invoice.AlreadyPaid",
            "This invoice is already paid. ");
    }
}
