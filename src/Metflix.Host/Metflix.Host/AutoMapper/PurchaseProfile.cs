using AutoMapper;
using Metflix.Models.DataflowModels;
using Metflix.Models.DbModels;
using Metflix.Models.DbModels.DbDtos;
using Metflix.Models.KafkaModels;
using Metflix.Models.Responses.Purchases.PurchaseDtos;

namespace Metflix.Host.AutoMapper
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile()
        {
            CreateMap<Purchase, PurchaseDto>();
            CreateMap<PurchaseInfoData, FullPurchaseInfoData>();
            CreateMap<UserMovieDbDto, UserMovieDto>();
        }
    }
}
