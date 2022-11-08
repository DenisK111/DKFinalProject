using AutoMapper;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;

namespace Metflix.Host.AutoMapper
{
    public class ReportsProfile : Profile
    {
        public ReportsProfile()
        {
            CreateMap<InventoryChangeData, InventoryLog>();
        }
    }
}
