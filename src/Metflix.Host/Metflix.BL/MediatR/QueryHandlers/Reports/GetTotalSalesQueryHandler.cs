using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Reports;
using Metflix.Models.Responses.Reports;

namespace Metflix.BL.MediatR.QueryHandlers.Reports
{
    public class GetTotalSalesQueryHandler : IRequestHandler<GetTotalSalesQuery, TotalSalesResponse>
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public GetTotalSalesQueryHandler(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public Task<TotalSalesResponse> Handle(GetTotalSalesQuery request, CancellationToken cancellationToken)
        {
            
            if (string.IsNullOrEmpty(request.request.EndDate))
            {

            }

            return null!;
        }
    }
}
