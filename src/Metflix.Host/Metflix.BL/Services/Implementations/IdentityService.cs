using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Metflix.BL.Services.Contracts;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.Requests.Identity;
using Metflix.Models.Responses.Users;
using Microsoft.AspNetCore.Identity;
using Utils;

namespace Metflix.BL.Services.Implementations
{
    public class IdentityService : IIDentityService
    {
        private readonly IIdentityRepository _userRepo;
        private readonly IMapper _mapper;

        public IdentityService(IIdentityRepository userRepo, IMapper mapper)
        {

            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserInfo?> CheckUserAndPassword(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userRepo.GetUserByEmail(email);

            if (user == null)
            {
                return null;
            }

            if (user.Password == CustomPasswordHasher.GetSHA512Password(password))
            {
                return user;
            }

            return null;
        }

        public async Task<RegisterUserResponse> CreateAsync(RegisterRequest userRequest, CancellationToken cancellationToken = default)
        {
            var userExists = await _userRepo.CheckIfUserExists(userRequest.Email);

            if (userExists)
            {
                return new RegisterUserResponse()
                {
                    HttpStatusCode=HttpStatusCode.Conflict,
                    Message = ResponseMessages.EmailExists,
                };
            }
            var newUser = _mapper.Map<UserInfo>(userRequest);
            var isCreated = await _userRepo.CreateUser(newUser);
            if (!isCreated)
            {
                return new RegisterUserResponse();
            }

            return new RegisterUserResponse()
            {
                HttpStatusCode = HttpStatusCode.Created
            };
        }

       

       
    }
}
