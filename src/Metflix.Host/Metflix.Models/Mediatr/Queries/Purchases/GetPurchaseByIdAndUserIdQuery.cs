using MediatR;
using Metflix.Models.Responses.Purchases;

namespace Metflix.Models.Mediatr.Queries.Purchases
{
    public record GetPurchaseByIdAndUserIdQuery(Guid Id,string UserId) : IRequest<PurchaseResponse>
    {
    }
}
