using System.Net;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Common;
using Metflix.Models.Mediatr.Queries.Purchases;
using Metflix.Models.Responses.Purchases;
using Metflix.Models.Responses.Purchases.PurchaseDtos;

namespace Metflix.BL.MediatR.QueryHandlers.Purchases
{
    public class GetPurchaseByIdAndUserIdQueryHandler : IRequestHandler<GetPurchaseByIdAndUserIdQuery,PurchaseResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseRepository _purchaseRepository;

        public GetPurchaseByIdAndUserIdQueryHandler(IPurchaseRepository purchaseRepository, IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
        }

        public async Task<PurchaseResponse> Handle(GetPurchaseByIdAndUserIdQuery request, CancellationToken cancellationToken)
        {
            var purchase = await _purchaseRepository.GetByIdAndUserId(request.Id,request.UserId,cancellationToken);
            if (purchase == null)
            {
                return new PurchaseResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.NoIdForUser
                };
            }
            var dto = _mapper.Map<PurchaseDto>(purchase);
            return new PurchaseResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = dto,
            };
        }
    }
}
