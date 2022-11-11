﻿using System.Net;
using AutoMapper;
using Metflix.BL.Services.Contracts;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Common;
using Metflix.Models.DbModels;
using Metflix.Models.Requests.Identity;
using Metflix.Models.Responses.Users;
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
            await _userRepo.CreateUser(newUser);            

            return new RegisterUserResponse()
            {
                HttpStatusCode = HttpStatusCode.Created,
                Message = ResponseMessages.SuccesfullyRegistered,
            };
        }    
              
    }
}
