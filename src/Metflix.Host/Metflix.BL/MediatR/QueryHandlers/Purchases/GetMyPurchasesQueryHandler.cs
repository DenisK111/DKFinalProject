using System.Net;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Purchases;
using Metflix.Models.Responses.Purchases;
using Metflix.Models.Responses.Purchases.PurchaseDtos;

namespace Metflix.BL.MediatR.QueryHandlers.Purchases
{
    public class GetMyPurchasesQueryHandler : IRequestHandler<GetMyPurchasesQuery, PurchaseCollectionResponse>
    {
        private readonly IPurchaseRepository _purchaseRepo;
        private readonly IMapper _mapper;

        public GetMyPurchasesQueryHandler(IPurchaseRepository purchaseRepo, IMapper mapper)
        {
            _purchaseRepo = purchaseRepo;
            _mapper = mapper;
        }

        public async Task<PurchaseCollectionResponse> Handle(GetMyPurchasesQuery request, CancellationToken cancellationToken)
        {
            var purchases = await _purchaseRepo.GetAllByUserId(request.UserId,cancellationToken);
            var purchaseDtos = _mapper.Map<IEnumerable<PurchaseDto>>(purchases);

            return new PurchaseCollectionResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = purchaseDtos
            };
        }
    }
}
