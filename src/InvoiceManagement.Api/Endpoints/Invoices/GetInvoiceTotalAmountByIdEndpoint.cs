using Humanizer;
using InvoiceManagement.Api.Common.Minimal;
using InvoiceManagement.Application.Invoices.GetInvoiceTotalAmountById;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace InvoiceManagement.Api.Endpoints.Invoices;

internal static class GetInvoiceTotalAmountByIdEndpoint
{
    internal static RouteHandlerBuilder MapGetInvoiceTotalAmountByIdEndpoint(this IEndpointRouteBuilder builder)
    {
        return builder.MapGet("/{invoiceId:guid}", HandleAsync)
            .WithName(nameof(GetInvoiceTotalAmountByIdEndpoint))
            .WithDisplayName(nameof(GetInvoiceTotalAmountByIdEndpoint).Humanize())
            .WithSummaryAndDescription(
                "Get total amount of an invoice by ID",
                "Retrieves the total amount for a specified invoice ID")
            .Produces<Ok<decimal?>>("Invoice total amount retrieved successfully.", StatusCodes.Status200OK)
            .ProducesValidationProblem("Invalid input for getting invoice total amount.")
            .ProducesProblem(
                "Internal server error while getting the invoice total amount.",
                StatusCodes.Status500InternalServerError)
            .MapToApiVersion(1.0);
    }

    private static async Task<Results<Ok<decimal?>, ValidationProblem, ProblemHttpResult>> HandleAsync(
        [AsParameters] GetInvoiceTotalAmountByIdRequestParameters requestParameters
    )
    {
        var (invoiceId, mediator, cancellationToken) = requestParameters;
        var query = new GetInvoiceTotalAmountByIdQuery(invoiceId);

        using (LogContext.PushProperty("Endpoint", nameof(GetInvoiceTotalAmountByIdEndpoint)))
        {
            var result = await mediator.Send(query, cancellationToken);
            return result.IsSuccess ?
                       TypedResults.Ok(result.Value) :
                       result.ToProblemDetails();
        }
    }
}

internal sealed record GetInvoiceTotalAmountByIdRequestParameters(
    [FromRoute]
    Guid InvoiceId,
    IMediator Mediator,
    CancellationToken CancellationToken
);
