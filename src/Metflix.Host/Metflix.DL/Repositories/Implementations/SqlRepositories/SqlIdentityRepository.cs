﻿using System.Data.SqlClient;
using Dapper;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.DbModels.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Metflix.DL.Repositories.Implementations.SqlRepositories
{
    public class SqlUserRepository : IUserRepository
    {

        private readonly ILogger<SqlMovieRepository> _logger;
        private readonly IOptionsMonitor<ConnectionStrings> _configuration;

        public SqlUserRepository(ILogger<SqlMovieRepository> logger, IOptionsMonitor<ConnectionStrings> configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken = default)
        {
            var query = @"SELECT COUNT(*) FROM USERS WITH (NOLOCK)
                            WHERE Email = @Email"
            ;

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    var result = await conn.ExecuteScalarAsync<int>(query, new { Email = email });
                    if (result > 0) return true;
                }
            }
            catch (Exception e)
            {
                e.Data.Add("IsCritical", true);
                e.Source = $"Error in {nameof(SqlUserRepository)}.{nameof(CheckIfUserExists)}";
                throw;
            }

            return false;
        }

        public async Task<bool> CreateUser(UserInfo user, CancellationToken cancellationToken = default)
        {
            var query = @"INSERT INTO Users
                        Values(@Id, @Name , @DateOfBirth, @Email, @Password,  GetDate(), GetDate(), @Role)";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    var result = await conn.ExecuteAsync(query, user);
                    if (result > 0) return true;
                }
            }
            catch (Exception e)
            {
                e.Data.Add("IsCritical", true);
                e.Source = $"Error in {nameof(SqlUserRepository)}.{nameof(CreateUser)}";
                throw;
            }

            return false;

        }

        public async Task<UserInfo> GetById(string id, CancellationToken cancellationToken = default)
        {
            var query = @"SELECT * FROM USERS WITH (NOLOCK)
                            WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryFirstAsync<UserInfo>(query, new { Id = id });                     
                }
            }
            catch (Exception e)
            {
                e.Data.Add("IsCritical", true);
                e.Source = $"Error in {nameof(SqlUserRepository)}.{nameof(GetById)}";                
                throw;
            }            
        }

        public async Task<UserInfo> GetUserByEmail(string email, CancellationToken cancellationToken = default)
        {
            var query = @"SELECT * FROM USERS WITH (NOLOCK)
                          WHERE Email = @Email";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryFirstAsync<UserInfo>(query, new { Email = email });                    
                }
            }
            catch (Exception e)
            {
                e.Data.Add("IsCritical", true);
                e.Source = $"Error in {nameof(SqlUserRepository)}.{nameof(GetUserByEmail)}";
                throw;
            }           
        }
    }
}
