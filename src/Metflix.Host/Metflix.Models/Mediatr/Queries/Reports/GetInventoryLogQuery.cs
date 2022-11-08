using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Metflix.Models.Responses.Reports;
using Metflix.Models.Responses.Reports.ReportDtos;

namespace Metflix.Models.Mediatr.Queries.Reports
{
    public class GetInventoryLogQuery : IRequest<InventoryLogResponse>
    {
    }
}
