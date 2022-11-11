using Metflix.Models.Responses;
using AutoMapper;
using Metflix.Host.AutoMapper;

namespace Metflix.Tests.Helpers
{
    public static class ResponseGenerator
    {
        private static readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MovieProfile());
            cfg.AddProfile(new PurchaseProfile());
            cfg.AddProfile(new UserProfile());
            cfg.AddProfile(new ReportsProfile());
        }).CreateMapper();

        public static TResponseType GenerateResponse<TResponseType, TModel, TMappingType>(HttpStatusCode statusCode, string message, TModel model)
            where TResponseType : BaseResponse<TMappingType>, new()
            where TMappingType : class
        {
            var modelDto = model?.Equals(default(TModel)) == null ? null : _mapper.Map<TMappingType>(model);

            return new TResponseType()
            {
                HttpStatusCode = statusCode,
                Message = message,
                Model = modelDto
            };
        }
    }
}
