using System.Globalization;
using AutoMapper;
using Metflix.Models.DbModels;
using Metflix.Models.DbModels.Helpers;
using Metflix.Models.Requests.Identity;
using Utils;

namespace Metflix.Host.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequest,UserInfo>()
                .ForMember(x=>x.Role,y=>y.MapFrom(s=>UserRoles.User))
                .ForMember(x=>x.DateOfBirth,y=>y.MapFrom(s=>DateTime.ParseExact(s.DateOfBirth,"yyyy-MM-dd",CultureInfo.InvariantCulture)))
                .ForMember(x=>x.Password,y=>y.MapFrom(s=>CustomPasswordHasher.GetSHA512Password(s.Password)));
        }
    }
}
