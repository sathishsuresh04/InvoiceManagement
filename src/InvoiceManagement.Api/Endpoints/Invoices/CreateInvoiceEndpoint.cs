using Humanizer;
using InvoiceManagement.Api.Common.Minimal;
using InvoiceManagement.Application.Invoices.CreateInvoice;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace InvoiceManagement.Api.Endpoints.Invoices;

internal static class CreateInvoiceEndpoint
{
    internal static RouteHandlerBuilder MapCreateInvoiceEndpoint(this IEndpointRouteBuilder builder)
    {
        return builder.MapPost("/", HandleAsync)
            .WithName(nameof(CreateInvoiceEndpoint))
            .WithDisplayName(nameof(CreateInvoiceEndpoint).Humanize())
            .WithSummaryAndDescription(
                "Create a new invoice",
                "Creates a new invoice with the provided details")
            .Produces<Ok>("Invoice created successfully.", StatusCodes.Status200OK)
            .ProducesValidationProblem("Invalid input for creating an invoice.")
            .ProducesProblem(
                "Internal server error while creating the invoice.",
                StatusCodes.Status500InternalServerError)
            .MapToApiVersion(1.0);
    }

    private static async Task<Results<Ok, ValidationProblem, ProblemHttpResult>> HandleAsync(
        [AsParameters] CreateInvoiceRequestParameters requestParameters
    )
    {
        var (createInvoiceRequest, mediator, cancellationToken) = requestParameters;
        var command = new CreateInvoiceCommand(
            createInvoiceRequest.Number,
            createInvoiceRequest.Seller,
            createInvoiceRequest.Buyer,
            createInvoiceRequest.Description,
            createInvoiceRequest.IssuedDate);

        using (LogContext.PushProperty("Endpoint", nameof(CreateInvoiceEndpoint)))
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSuccess ?
                       TypedResults.Ok() :
                       result.ToProblemDetails();
        }
    }
}

public record CreateInvoiceRequestDto(
    string Number,
    string Seller,
    string Buyer,
    string? Description,
    DateTime IssuedDate
);

internal sealed record CreateInvoiceRequestParameters(
    [FromBody]
    CreateInvoiceRequestDto CreateInvoiceRequest,
    IMediator Mediator,
    CancellationToken CancellationToken
);
