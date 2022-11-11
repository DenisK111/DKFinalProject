using MediatR;
using Metflix.Models.Responses.Purchases;

namespace Metflix.Models.Mediatr.Queries.Purchases
{
    public record GetMyPurchasesQuery(string UserId) : IRequest<PurchaseCollectionResponse>
    {
    }
}
