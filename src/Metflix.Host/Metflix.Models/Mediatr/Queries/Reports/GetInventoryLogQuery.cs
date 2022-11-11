using MediatR;
using Metflix.Models.Responses.Reports;

namespace Metflix.Models.Mediatr.Queries.Reports
{
    public class GetInventoryLogQuery : IRequest<InventoryLogResponse>
    {
    }
}
