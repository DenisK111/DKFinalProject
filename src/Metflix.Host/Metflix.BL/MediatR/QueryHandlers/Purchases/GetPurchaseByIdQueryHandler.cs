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
    public class GetPurchaseByIdQueryHandler : IRequestHandler<GetPurchaseByIdQuery, PurchaseResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseRepository _purchaseRepository;

        public GetPurchaseByIdQueryHandler(IMapper mapper, IPurchaseRepository purchaseRepository)
        {
            _mapper = mapper;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<PurchaseResponse> Handle(GetPurchaseByIdQuery request, CancellationToken cancellationToken)
        {
            var purchase = await _purchaseRepository.GetById(request.Id,cancellationToken);
            if (purchase == null)
            {
                return new PurchaseResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.IdNotFound
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
