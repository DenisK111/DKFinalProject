using System.Net;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Common;
using Metflix.Models.Mediatr.Commands.Purchases;
using Metflix.Models.Responses.Movies.MovieDtos;
using Metflix.Models.Responses.Purchases;

namespace Metflix.BL.MediatR.CommandHandlers.Purchases
{
    public class ReturnMovieCommandHandler : IRequestHandler<ReturnMovieCommand, ReturnMovieResponse>
    {
        private readonly IUserMovieRepository _userMovieRepository;
        private readonly IMovieRepository _movierepository;
        private readonly IMapper _mapper;

        public ReturnMovieCommandHandler(IUserMovieRepository userMovieRepository, IMovieRepository movierepository, IMapper mapper)
        {
            _userMovieRepository = userMovieRepository;
            _movierepository = movierepository;
            _mapper = mapper;
        }

        public async Task<ReturnMovieResponse> Handle(ReturnMovieCommand request, CancellationToken cancellationToken)
        {
            var userMovie = await _userMovieRepository.GetById(request.request.UserMovieId, cancellationToken);
            if (userMovie == null)
            {
                return new ReturnMovieResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.IdNotFound
                };
            }

            if (userMovie.IsReturned)
            {
                return new ReturnMovieResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessages.MovieAlreadyReturned
                };
            }

            await _userMovieRepository.MarkAsReturned(request.request.UserMovieId, cancellationToken);           
            await _movierepository.IncreaseAvailableQuantity(userMovie.MovieId, cancellationToken:cancellationToken);           

            var movie = await _movierepository.GetById(request.request.UserMovieId, cancellationToken);

            return new ReturnMovieResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = _mapper.Map<MovieDto>(movie)
            };
        }
    }
}
