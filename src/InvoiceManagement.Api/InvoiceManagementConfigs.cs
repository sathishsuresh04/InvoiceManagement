using InvoiceManagement.Api.Common;
using InvoiceManagement.Api.Endpoints.Invoices;

namespace InvoiceManagement.Api;

public static class InvoiceManagementConfigs
{
    private const string InvoicesTag = "Invoices";
    private const string InvoiceItems = "InvoiceItems";
    private const string Invoices = "invoices";
    private static string InvoicePrefixUri => $"{EndpointConfig.BaseApiPath}/{Invoices}";
    private static string InvoiceItemsPrefixUri => InvoicePrefixUri + "/{id:guid}/invoice-items";

    public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", () => "InvoiceManagement.Api").ExcludeFromDescription();
        endpoints.MapInvoiceModuleEndpoints();
        endpoints.MapInvoiceItemModuleEndpoints();
        return endpoints;
    }

    private static IEndpointRouteBuilder MapInvoiceModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var invoiceV1 = endpoints.NewVersionedApi(InvoicesTag)
            .MapGroup(InvoicePrefixUri)
            .HasDeprecatedApiVersion(0.9)
            .HasApiVersion(1.0);

        invoiceV1.MapCreateInvoiceEndpoint();
        invoiceV1.MapGetInvoiceTotalAmountByIdEndpoint();
        invoiceV1.MapGetItemsReportEndpoint();
        invoiceV1.MapGetTotalOfUnpaidEndpoint();

        return endpoints;
    }

    private static IEndpointRouteBuilder MapInvoiceItemModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        //Add any invoice items specific endpoints
        var invooiceItemsV1 = endpoints.NewVersionedApi(InvoiceItems)
            .MapGroup(InvoiceItemsPrefixUri)
            .HasDeprecatedApiVersion(0.9)
            .HasApiVersion(1.0);
        return endpoints;
    }
}
