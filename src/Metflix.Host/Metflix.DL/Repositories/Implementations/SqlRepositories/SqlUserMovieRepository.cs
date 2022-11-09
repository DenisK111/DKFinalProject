using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.DbModels.Configurations;
using Metflix.Models.Responses.Purchases.PurchaseDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace Metflix.DL.Repositories.Implementations.SqlRepositories
{
    public class SqlUserMovieRepository : IUserMovieRepository
    {

        private readonly ILogger<SqlUserMovieRepository> _logger;
        private readonly IOptionsMonitor<ConnectionStrings> _configuration;

        public SqlUserMovieRepository(ILogger<SqlUserMovieRepository> logger, IOptionsMonitor<ConnectionStrings> configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<int> Add(UserMovie model, CancellationToken cancellationToken = default)
        {
            var query = @"INSERT INTO UserMovies
                          VALUES(@UserId,@MovieId,GetDate(),DateAdd(Day,@DaysFor,GetDate()),@IsReturned,GetDate(),@DaysFor)
                          SELECT CAST(SCOPE_IDENTITY() as int)";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    var result = await conn.QuerySingleAsync<int>(query, new
                    {
                        UserId = model.UserId,
                        MovieId = model.MovieId,                        
                        IsReturned = model.IsReturned,
                        DaysFor = model.DaysFor
                    });

                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Add)}:{e.Message}", e);
                throw;
            }
        }
        public async Task<IEnumerable<UserMovie>> GetAll(CancellationToken cancellationToken = default)
        {
            var query = @"SELECT * FROM UserMovies WITH (NOLOCK)";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryAsync<UserMovie>(query);
                }
            }

            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAll)}:{e.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UserMovie>> GetAllByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var query = @"SELECT * FROM USERMOVIES WITH (NOLOCK)
                            WHERE UserId = @UserId";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryAsync<UserMovie>(query, new { UserId = userId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllByUserId)}:{e.Message}", e);
                throw;
            }
        }

        public async Task<IEnumerable<UserMovieDto>> GetAllUnreturnedByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var query = @"SELECT m.Name,um.Id,um.DueDate FROM UserMovies as um
                            JOIN Movies as m ON m.Id = um.MOVIEID
                            WHERE um.USERID = @userId AND um.IsReturned = 0;";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryAsync<UserMovieDto>(query, new { UserId = userId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllUnreturnedByUserId)}:{e.Message}", e);
                throw;
            }
        }

        public async Task<IEnumerable<UserMovie>> GetAllOverDue(CancellationToken cancellationToken = default)
        {
            var query = @"SELECT * FROM UserMovies
                          WHERE GETDATE() > DueDate AND IsReturned = 0
                          Order By DueDate";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryAsync<UserMovie>(query);
                }
            }

            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllOverDue)}:{e.Message}");
                throw;
            }

        }

        public async Task<UserMovie?> GetById(int id, CancellationToken cancellationToken = default)
        {
            var query = @"SELECT * FROM UserMovies WITH (NOLOCK) WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryFirstOrDefaultAsync<UserMovie>(query, new { Id = id });

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetById)}:{e.Message}", e);
                throw;
            }
        }


        public async Task<bool> MarkAsReturned(int id, CancellationToken cancellationToken = default)
        {
            var query = @"Update UserMovies
                          SET IsReturned=1
                          WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    await conn.ExecuteAsync(query, new { Id = id });
                    return true;
                }
            }

            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(MarkAsReturned)}:{e.Message}", e);
                throw;
            }
        }

        public async Task<UserMovie?> Update(UserMovie model, CancellationToken cancellationToken = default)
        {
            var query = @"UPDATE UserMovies
                        SET UserId = @UserId, MovieId = @MovieId, LastChanged = GetDate(), DueDate = @DueDate, IsReturned = @IsReturned, DaysFor = @DaysFor
                        WHERE Id = @Id
                        SELECT * FROM UserMovies
                        WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    var result = await conn.QuerySingleAsync<UserMovie>(query, model);
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Update)}:{e.Message}", e);
                throw;
            }
        }
    }
}
