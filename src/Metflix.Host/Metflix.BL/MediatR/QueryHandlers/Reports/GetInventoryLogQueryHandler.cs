using System.Text;
using MediatR;
using Metflix.BL.Extensions;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Reports;
using Metflix.Models.Responses.Reports;
using Metflix.Models.Responses.Reports.ReportDtos;

namespace Metflix.BL.MediatR.QueryHandlers.Reports
{
    public class GetInventoryLogQueryHandler : IRequestHandler<GetInventoryLogQuery, InventoryLogResponse>
    {
        private readonly IInventoryLogRepository _logRepository;

        public GetInventoryLogQueryHandler(IInventoryLogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<InventoryLogResponse> Handle(GetInventoryLogQuery request, CancellationToken cancellationToken)
        {
            var logs = await _logRepository.GetAll();

            var sb = new StringBuilder();

            foreach (var log in logs)
            {
                sb.AppendLine(log.ToLogString());
            }

            return new InventoryLogResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Model = new InventoryLogDto()
                {
                    Log = sb.ToString().TrimEnd()
                }
            };
        }
    }
}
