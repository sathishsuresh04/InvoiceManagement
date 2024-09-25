using Humanizer;
using InvoiceManagement.Api.Common.Minimal;
using InvoiceManagement.Application.Invoices.GetTotalOfUnpaid;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog.Context;

namespace InvoiceManagement.Api.Endpoints.Invoices;

internal static class GetTotalOfUnpaidEndpoint
{
    internal static RouteHandlerBuilder MapGetTotalOfUnpaidEndpoint(this IEndpointRouteBuilder builder)
    {
        return builder.MapGet("/unpaid-total", HandleAsync)
            .WithName(nameof(GetTotalOfUnpaidEndpoint))
            .WithDisplayName(nameof(GetTotalOfUnpaidEndpoint).Humanize())
            .WithSummaryAndDescription(
                "Get total of unpaid invoices",
                "Retrieves the total amount of all unpaid invoices")
            .Produces<Ok<decimal>>("Total of unpaid invoices retrieved successfully.", StatusCodes.Status200OK)
            .ProducesValidationProblem("Invalid input for getting the total of unpaid invoices.")
            .ProducesProblem(
                "Internal server error while getting the total of unpaid invoices.",
                StatusCodes.Status500InternalServerError)
            .MapToApiVersion(1.0);
    }

    private static async Task<Results<Ok<decimal>, ProblemHttpResult>> HandleAsync(
        [AsParameters] GetTotalOfUnpaidRequestParameters requestParameters
    )
    {
        var (mediator, cancellationToken) = requestParameters;
        var query = new GetTotalOfUnpaidQuery();

        using (LogContext.PushProperty("Endpoint", nameof(GetTotalOfUnpaidEndpoint)))
        {
            var result = await mediator.Send(query, cancellationToken);
            return result.IsSuccess ?
                       TypedResults.Ok(result.Value) :
                       result.ToProblemDetails();
        }
    }
}

internal sealed record GetTotalOfUnpaidRequestParameters(
    IMediator Mediator,
    CancellationToken CancellationToken
);
