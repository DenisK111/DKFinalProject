using System.Globalization;
using System.Net;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Reports;
using Metflix.Models.Responses.Reports;
using Metflix.Models.Responses.Reports.ReportDtos;

namespace Metflix.BL.MediatR.QueryHandlers.Reports
{
    public class GetTotalSalesQueryHandler : IRequestHandler<GetTotalSalesQuery, TotalSalesResponse>
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public GetTotalSalesQueryHandler(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<TotalSalesResponse> Handle(GetTotalSalesQuery request, CancellationToken cancellationToken)
        {
            var startDate = DateTime.Parse(request.request.StartDate,CultureInfo.InvariantCulture);
            var endDate = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(request.request.EndDate))
            {
                endDate = DateTime.Parse(request.request.EndDate, CultureInfo.InvariantCulture);
            }

            var totalSales = await _purchaseRepository.GetTotalSales(startDate, endDate,cancellationToken);

            return new TotalSalesResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = new TotalSalesDto()
                {
                    TotalSales = totalSales
                }
            };            
        }
    }
}
