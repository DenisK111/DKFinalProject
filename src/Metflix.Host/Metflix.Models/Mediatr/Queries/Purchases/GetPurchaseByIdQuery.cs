using MediatR;
using Metflix.Models.Responses.Purchases;

namespace Metflix.Models.Mediatr.Queries.Purchases
{
    public record GetPurchaseByIdQuery(Guid Id) : IRequest<PurchaseResponse>
    {
    }
}
