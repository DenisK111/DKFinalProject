using MediatR;
using Metflix.Models.Requests.Reports;
using Metflix.Models.Responses.Reports;

namespace Metflix.Models.Mediatr.Queries.Reports
{
    public record GetTotalSalesQuery(TimePeriodRequest request) : IRequest<TotalSalesResponse>
    {
    }
}
