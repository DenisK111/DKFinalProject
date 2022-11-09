using AutoMapper;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;
using Metflix.Models.Responses.Reports.ReportDtos;

namespace Metflix.Host.AutoMapper
{
    public class ReportsProfile : Profile
    {
        public ReportsProfile()
        {
            CreateMap<InventoryChangeData, InventoryLog>();
            CreateMap<UserMovie, UserMovieOverDueDto>();
        }
    }
}
