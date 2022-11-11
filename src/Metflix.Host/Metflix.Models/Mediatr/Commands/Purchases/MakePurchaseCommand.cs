using MediatR;
using Metflix.Models.Requests.Purchase;
using Metflix.Models.Responses.Purchases;

namespace Metflix.Models.Mediatr.Commands.Purchases
{
    public record MakePurchaseCommand(PurchaseRequest Request, string UserId) : IRequest<PurchaseResponse>
    {
    }
}
