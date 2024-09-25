using Humanizer;
using InvoiceManagement.Api.Common.Minimal;
using InvoiceManagement.Application.Invoices.GetItemsReport;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace InvoiceManagement.Api.Endpoints.Invoices;

internal static class GetItemsReportEndpoint
{
    internal static RouteHandlerBuilder MapGetItemsReportEndpoint(this IEndpointRouteBuilder builder)
    {
        return builder.MapGet("/items-report", HandleAsync)
            .WithName(nameof(GetItemsReportEndpoint))
            .WithDisplayName(nameof(GetItemsReportEndpoint).Humanize())
            .WithSummaryAndDescription(
                "Get items report",
                "Retrieves a report of items within the specified date range")
            .Produces<Ok<IReadOnlyDictionary<string, long>>>(
                "Items report retrieved successfully.",
                StatusCodes.Status200OK)
            .ProducesValidationProblem("Invalid input for getting the items report.")
            .ProducesProblem(
                "Internal server error while getting the items report.",
                StatusCodes.Status500InternalServerError)
            .MapToApiVersion(1.0);
    }

    private static async Task<Results<Ok<IReadOnlyDictionary<string, long>>, ProblemHttpResult>> HandleAsync(
        [AsParameters] GetItemsReportRequestParameters requestParameters
    )
    {
        var (fromDate, toDate, mediator, cancellationToken) = requestParameters;
        var query = new GetItemsReportQuery(fromDate, toDate);

        using (LogContext.PushProperty("Endpoint", nameof(GetItemsReportEndpoint)))
        {
            var result = await mediator.Send(query, cancellationToken);
            return result.IsSuccess ?
                       TypedResults.Ok(result.Value) :
                       result.ToProblemDetails();
        }
    }
}

internal sealed record GetItemsReportRequestParameters(
    [FromQuery]
    DateTime? FromDate,
    [FromQuery]
    DateTime? ToDate,
    IMediator Mediator,
    CancellationToken CancellationToken
);
