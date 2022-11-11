using System.Net;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Movies;
using Metflix.Models.Responses.Movies;
using Metflix.Models.Responses.Movies.MovieDtos;

namespace Metflix.BL.MediatR.QueryHandlers.Movies
{
    public class GetAvailableMoviesQueryHandler : IRequestHandler<GetAvailableMoviesQuery, AvailableMoviesResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetAvailableMoviesQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<AvailableMoviesResponse> Handle(GetAvailableMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository.GetAllAvailableMovies();
            var movieDtos = _mapper.Map<IEnumerable<AvailableMovieDto>>(movies);

            return new AvailableMoviesResponse()
            {
                Model = movieDtos,
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
    }
}
