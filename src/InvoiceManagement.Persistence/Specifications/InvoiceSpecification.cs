using System.Linq.Expressions;
using InvoiceManagement.Domain.Invoices;

namespace InvoiceManagement.Persistence.Specifications;

internal sealed class InvoiceSpecification : Specification<Invoice>
{
    internal override Expression<Func<Invoice, bool>> ToExpression()
    {
        return invoice => true;
    }
}
