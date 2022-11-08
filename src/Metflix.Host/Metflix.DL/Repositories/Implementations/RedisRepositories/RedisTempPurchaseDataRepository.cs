using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DnsClient.Internal;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Metflix.DL.Repositories.Implementations.RedisRepositories
{
    public class RedisTempPurchaseDataRepository : ITempPurchaseDataRepository
    {
        private readonly IDatabase _database;
        private readonly ILogger<RedisTempPurchaseDataRepository> _logger;

        public RedisTempPurchaseDataRepository(IOptionsMonitor<RedisSettings> _redisSettings,ILogger<RedisTempPurchaseDataRepository> logger,IConnectionMultiplexer connection)
        {
            _logger = logger;            
            _database = connection.GetDatabase();           
        }

        public async Task SetOrUpdateEntryAsync(string key, byte[] value)
        {
            try
            {
               await _database.StringSetAsync(key,value);                
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}\r\n{e.StackTrace}");
                throw;
            }
        }

        public async Task DeleteEntryAsync(string key)
        {
            try
            {
                await _database.StringGetDeleteAsync(key);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}\r\n{e.StackTrace}");
                throw;
            }
        }

        public async Task<byte[]> GetValueAsync(string key)
        {
            try
            {
               var value =  await _database.StringGetAsync(key)!;                
               return value!;
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}\r\n{e.StackTrace}");
                throw;
            }
        }

        public async Task<bool> ContainsKeyAsync(string key)
        {
            try
            {
                return await _database.KeyExistsAsync(key);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}\r\n{e.StackTrace}");
                throw;
            }
        }
    }
}
