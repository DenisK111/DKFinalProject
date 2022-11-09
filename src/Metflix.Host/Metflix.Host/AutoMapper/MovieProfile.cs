using System.Runtime.CompilerServices;
using AutoMapper;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;
using Metflix.Models.Requests.Movies;
using Metflix.Models.Responses.Movies.MovieDtos;

namespace Metflix.Host.AutoMapper
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<AddMovieRequest, Movie>();
            CreateMap<UpdateMovieRequest, Movie>();
            CreateMap<Movie, MovieDto>();
            CreateMap<Movie, AvailableMovieDto>();
            CreateMap<Movie, MovieRecord>();
            CreateMap<Movie, MovieInfoData>();
            CreateMap<MovieInfoData, MovieRecord>();
        }
    }
}
