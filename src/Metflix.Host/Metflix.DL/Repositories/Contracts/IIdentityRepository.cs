﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;

namespace Metflix.DL.Repositories.Contracts
{
    public interface IIdentityRepository
    {
        Task<UserInfo?> GetUserByEmail(string email, CancellationToken cancellationToken = default);

        Task<bool> CreateUser(UserInfo user, CancellationToken cancellationToken = default);

        Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken = default);
    }
}
