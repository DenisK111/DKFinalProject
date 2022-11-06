using AutoMapper;
using Metflix.Models.DbModels;
using Metflix.Models.Responses.Purchases.PurchaseDtos;

namespace Metflix.Host.AutoMapper
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile()
        {
            CreateMap<Purchase, PurchaseDto>();
        }
    }
}
