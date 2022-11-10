using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;
using Metflix.Models.Responses.Movies.MovieDtos;
using Metflix.Models.Responses.Movies;
using Metflix.Models.Responses;
using AutoMapper;
using Metflix.Host.AutoMapper;

namespace Metflix.Tests.Helpers
{
    public class ResponseGenerator
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
