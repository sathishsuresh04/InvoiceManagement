using FluentValidation.Results;
using InvoiceManagement.Domain.Primitives;

namespace InvoiceManagement.Application.Core.Exceptions;

public sealed class ValidationException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ValidationException" /> class.
    /// </summary>
    /// <param name="failures">The collection of validation failures.</param>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures has occurred.")
    {
        Errors = failures
            .Distinct()
            .Select(failure => Error.Failure(failure.ErrorCode, failure.ErrorMessage))
            .ToList();
    }

    /// <summary>
    ///     Gets the validation errors.
    /// </summary>
    public IReadOnlyCollection<Error> Errors { get; }
}
