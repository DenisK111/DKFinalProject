using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;
using Metflix.Models.Requests.Identity;
using Metflix.Models.Responses.Users;
using Microsoft.AspNetCore.Identity;

namespace Metflix.BL.Services.Contracts
{
    public interface IIDentityService 
    {
        Task<RegisterUserResponse> CreateAsync(RegisterRequest userRequest,CancellationToken cancellationToken = default);

        Task<UserInfo?> CheckUserAndPassword(string email,string password, CancellationToken cancellationToken = default);               

    }
}
