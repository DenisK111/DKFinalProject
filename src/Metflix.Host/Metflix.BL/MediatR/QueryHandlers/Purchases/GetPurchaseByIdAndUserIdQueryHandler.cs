using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Purchases;
using Metflix.Models.Responses.Purchases;
using Metflix.Models.Responses.Purchases.PurchaseDtos;
using Utils;

namespace Metflix.BL.MediatR.QueryHandlers.Purchases
{
    public class GetPurchaseByIdAndUserIdQueryHandler : IRequestHandler<GetPurchaseByIdAndUserIdQuery,PurchaseResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseRepository _purchaseRepository;

        public GetPurchaseByIdAndUserIdQueryHandler(IMapper mapper, IPurchaseRepository purchaseRepository)
        {
            _mapper = mapper;
            _purchaseRepository = purchaseRepository;
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
