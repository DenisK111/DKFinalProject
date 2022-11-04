﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.DbModels.Configurations;
using Metflix.Models.Responses.Movies.MovieDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        public async Task<Movie?> Add(Movie model, CancellationToken cancellationToken = default)
        {
            var query = @"INSERT INTO Movies ([Name],TotalQuantity,AvailableQuantity,PricePerDay,LastChanged,Year)
                            VALUES (@Name,@TotalQuantity,@TotalQuantity,@PricePerDay,GetDate(),@Year)
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
                _logger.LogError($"Error in {nameof(Add)}:{e.Message}", e);
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
                    await conn.ExecuteAsync(query, new { Id = modelId });
                    return true;
                }
            }

            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Delete)}:{e.Message}", e);
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
                _logger.LogError($"Error in {nameof(GetAll)}:{e.Message}");
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
                _logger.LogError($"Error in {nameof(GetAllAvailableMovies)}:{e.Message}", e);
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
                _logger.LogError($"Error in {nameof(GetById)}:{e.Message}", e);
                throw;
            }
        }

        public async Task<Movie?> Update(Movie model, CancellationToken cancellationToken = default)
        {
            var query = @"UPDATE Movies
                        SET [Name] = @Name, TotalQuantity = @TotalQuantity, AvailableQuantity = @AvailableQuantity, PricePerDay = @PricePerDay, LastChanged = GetDate(),Year = @Year
                        WHERE Id = @Id
                        SELECT * FROM Movies
                        WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.CurrentValue.SqlConnection))
                {
                    await conn.OpenAsync(cancellationToken);
                    var result = await conn.QuerySingleAsync<Movie>(query, model);
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
