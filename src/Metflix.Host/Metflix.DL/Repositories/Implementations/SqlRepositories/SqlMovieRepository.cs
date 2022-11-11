using System.Data.SqlClient;
using Dapper;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.DbModels.Configurations;
using Metflix.Models.Responses.Movies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utils;

namespace Metflix.DL.Repositories.Implementations.SqlRepositories
{
    public class SqlMovieRepository : IMovieRepository
    {

        private readonly ILogger<SqlMovieRepository> _logger;
        private readonly IOptionsMonitor<ConnectionStrings> _configuration;

        public SqlMovieRepository(ILogger<SqlMovieRepository> logger, IOptionsMonitor<ConnectionStrings> configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<Movie> Add(Movie model, CancellationToken cancellationToken = default)
        {
            var query = @"INSERT INTO Movies ([Name],TotalQuantity,AvailableQuantity,PricePerDay,LastChanged,Year)
                            VALUES (@Name,@TotalQuantity,@AvailableQuantity,@PricePerDay,GetDate(),@Year)
                             SELECT CAST(SCOPE_IDENTITY() as int)";
            try
            {
              
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    var result = await conn.QuerySingleAsync<int>(query, model);
                    model.Id = result;
                    return model;
                }
            }
            catch (Exception e)
            {
                
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(Add)}";                
                throw;
            }
        }

        public async Task<bool> Delete(int modelId, CancellationToken cancellationToken = default)
        {
            var query = @"DELETE FROM Movies
                          WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    var result = await conn.ExecuteAsync(query, new { Id = modelId });
                    return result == 1;
                }
            }

            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(Delete)}";
                throw;
            }
        }

        public async Task<IEnumerable<Movie>> GetAll(CancellationToken cancellationToken = default)
        {
            var query = @"SELECT * FROM MOVIES WITH (NOLOCK)";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryAsync<Movie>(query);

                }
            }

            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(GetAll)}";
                throw;
            }
        }

        public async Task<IEnumerable<Movie>> GetAllAvailableMovies(CancellationToken cancellationToken = default)
        {
            var query = @"SELECT Id, Name, Year, PricePerDay
                            From Movies WITH (NOLOCK)
                            WHERE AvailableQuantity > 0";


            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryAsync<Movie>(query);

                }
            }
            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(GetAllAvailableMovies)}";
                throw;
            }
        }
        public async Task<Movie?> GetById(int id, CancellationToken cancellationToken = default)
        {

            var query = @"SELECT * FROM Movies WITH (NOLOCK) WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QueryFirstOrDefaultAsync<Movie>(query, new { Id = id });

                }
            }
            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(GetById)}";
                throw;
            }
        }

        public async Task IncreaseAvailableQuantity(int movieId, int amount = 1, CancellationToken cancellationToken = default)
        {
            var query = @"UPDATE Movies
                          SET AvailableQuantity = AvailableQuantity + @Amount,
                              LastChanged = GetDate()
                          WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    await conn.ExecuteAsync(query, new { Id = movieId, Amount = amount });
                }
            }
            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(IncreaseAvailableQuantity)}";
                throw;
            }
        }

        public async Task DecreaseAvailableQuantity(int movieId, int amount = 1, CancellationToken cancellationToken = default)
        {
            var query = @"UPDATE Movies
                          SET AvailableQuantity = AvailableQuantity - @Amount,
                              LastChanged = GetDate()
                          WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    await conn.ExecuteAsync(query, new { Id = movieId, Amount = amount });
                }
            }
            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(DecreaseAvailableQuantity)}";
                throw;
            }
        }

        public async Task<Movie> Update(Movie model, CancellationToken cancellationToken = default)
        {
            var query = @"UPDATE Movies
                        SET [Name] = @Name,
                            TotalQuantity = @TotalQuantity,
                            AvailableQuantity = @AvailableQuantity,
                            PricePerDay = @PricePerDay,
                            LastChanged = GetDate(),
                            Year = @Year
                        WHERE Id = @Id
                        SELECT * FROM Movies WITH (NOLOCK)
                        WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QuerySingleAsync<Movie>(query, model);                    
                }
            }
            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(Update)}";
                throw;
            }
        }

        public async Task<Movie> AdjustInventory(int movieId, int amount, CancellationToken cancellationToken = default)
        {
            var query = @"UPDATE Movies
                        SET AvailableQuantity = AvailableQuantity + @Amount,
                        TotalQuantity = TotalQuantity + @Amount,
                        LastChanged = GetDate()
                        WHERE ID = @Id
                        SELECT * FROM Movies WITH (NOLOCK)
                        WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.QuerySingleAsync<Movie>(query, new { Id = movieId, Amount = amount });
                }
            }
            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(AdjustInventory)}";
                throw;
            }
        }

        public async Task<int> GetCurrentQuantity(int movieId, CancellationToken cancellationToken = default)
        {
            var query = @"SELECT AvailableQuantity 
                          FROM MOVIES WITH (NOLOCK)
                          WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.ExecuteScalarAsync<int>(query, new { Id = movieId });
                }
            }
            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(GetCurrentQuantity)}";
                throw;
            }
        }

        public async Task<int> GetTotalQuantity(int movieId, CancellationToken cancellationToken = default)
        {
            var query = @"SELECT TotalQuantity
                          FROM MOVIES 
                          WITH (NOLOCK)
                          WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    return await conn.ExecuteScalarAsync<int>(query, new { Id = movieId });
                }
            }
            catch (Exception e)
            {
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(GetTotalQuantity)}";
                throw;
            }
        }

        public async Task<Movie> IncreaseAvailableQuantityMarkAsReturnedAndGetMovieByIdTransaction(int userMovieId, int movieId, CancellationToken cancellationToken = default)
        {
            var increaseQuantityQuery = @"UPDATE Movies                                          
                                          SET AvailableQuantity = AvailableQuantity + 1,
                                              LastChanged = GetDate()
                                          OUTPUT INSERTED.*
                                          WHERE Id = @Id";

            var markAsReturnedQuery = @"Update UserMovies
                                        SET IsReturned = 1,
                                            LastChanged = GetDate()
                                        WHERE Id = @Id";
            Movie response = default(Movie)!;
            try
            {
                using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            response = await conn.QuerySingleOrDefaultAsync<Movie>(increaseQuantityQuery, new {Id = movieId}, transaction: transaction);
                            await conn.ExecuteAsync(markAsReturnedQuery, new {Id = userMovieId}, transaction: transaction);

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
                e.Source = $"Error in {nameof(SqlMovieRepository)}.{nameof(IncreaseAvailableQuantityMarkAsReturnedAndGetMovieByIdTransaction)}";
                throw;
            }

            return response;
        }
    }
}
