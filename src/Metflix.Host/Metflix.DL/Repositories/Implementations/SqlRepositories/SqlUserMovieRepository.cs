using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.DbModels.Configurations;
using Metflix.Models.DbModels.DbDtos;
using Metflix.Models.Responses.Purchases.PurchaseDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Utils;
using static MongoDB.Driver.WriteConcern;

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
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(Add)}";
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
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(GetAll)}";
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
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(GetAllByUserId)}";
                throw;
            }
        }

        public async Task<IEnumerable<UserMovieDbDto>> GetAllUnreturnedByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var query = @"SELECT m.Name,um.Id,um.DueDate FROM UserMovies as um
                            JOIN Movies as m ON m.Id = um.MOVIEID
                            WHERE um.USERID = @userId AND um.IsReturned = 0;";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryAsync<UserMovieDbDto>(query, new { UserId = userId });
                }
            }
            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(GetAllUnreturnedByUserId)}";
                throw;
            }
        }

        public async Task<IEnumerable<UserMovie>> GetAllOverDue(CancellationToken cancellationToken = default)
        {
            var query = @"SELECT * FROM UserMovies WITH (NOLOCK)
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
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(GetAllOverDue)}";
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
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(GetById)}";
                throw;
            }
        }


        public async Task MarkAsReturned(int id, CancellationToken cancellationToken = default)
        {
            var query = @"Update UserMovies
                          SET IsReturned = 1,
                              LastChanged = GetDate()
                          WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    await conn.ExecuteAsync(query, new { Id = id });
                }
            }

            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(MarkAsReturned)}";
                throw;
            }
        }

        public async Task<UserMovie?> Update(UserMovie model, CancellationToken cancellationToken = default)
        {
            var query = @"UPDATE UserMovies
                        SET UserId = @UserId, MovieId = @MovieId, LastChanged = GetDate(), DueDate = @DueDate, IsReturned = @IsReturned, DaysFor = @DaysFor
                        WHERE Id = @Id
                        SELECT * FROM UserMovies WITH (NOLOCK)
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
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(Update)}";
                throw;
            }
        }

        public async Task<IEnumerable<int>> DecreaseAvailableQuantityAndAddUserMoviesTransaction(List<UserMovie> userMovies, CancellationToken cancellationToken = default)
        {
            var queryInsert = @"INSERT INTO UserMovies
                                OUTPUT INSERTED.Id
                                VALUES(@UserId,@MovieId,GetDate(),DateAdd(Day,@DaysFor,GetDate()),@IsReturned,GetDate(),@DaysFor)";

            var queryDecreaseQuantity = @"UPDATE Movies
                          SET AvailableQuantity = AvailableQuantity - 1,
                              LastChanged = GetDate()
                          WHERE Id = @MovieId";

            var ids = new List<int>();
            try
            {
                using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            await conn.ExecuteAsync(queryDecreaseQuantity, userMovies, transaction: transaction);

                            var count = userMovies.Count();
                            for (int i = 0; i < count; i++)
                            {
                                var id = await conn.QuerySingleAsync<int>(queryInsert, userMovies[i], transaction: transaction);
                                ids.Add(id);
                            }

                            transaction.Commit();
                        }                                             

                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.Data.Add(ExceptionDataKeys.IsCritical, true);
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(DecreaseAvailableQuantityAndAddUserMoviesTransaction)}";
                throw;
            }

            return ids;
        }

        public async Task ReverseDecreaseAvailableQuantityAndAddUserMoviesTransaction(IEnumerable<int> userMovieIds, IEnumerable<int> movieIds,CancellationToken cancellationToken = default)
        {
            var queryDelete = @"DELETE FROM UserMovies
                                Where Id = @Id";

            var queryIncreaseQuantity = @"UPDATE Movies
                                         SET AvailableQuantity = AvailableQuantity + 1,
                                             LastChanged = GetDate()
                                         WHERE Id = @MovieId";

            var ids = new List<int>();
            try
            {
                using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            await conn.ExecuteAsync(queryIncreaseQuantity, movieIds.Select(x => new {MovieId = x}), transaction: transaction);
                            await conn.ExecuteAsync(queryDelete, userMovieIds.Select(x => new { Id = x }), transaction: transaction);                            

                            transaction.Commit();
                        }

                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.Data.Add(ExceptionDataKeys.IsCritical, true);
                e.Source = $"Error in {nameof(SqlUserMovieRepository)}.{nameof(DecreaseAvailableQuantityAndAddUserMoviesTransaction)}";
                throw;
            }            
        }
    }

   
}

