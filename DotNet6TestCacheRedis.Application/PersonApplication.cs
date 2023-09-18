using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DotNet6TestCacheRedis.Domain;
using Microsoft.Extensions.Caching.Distributed;

namespace DotNet6TestCacheRedis.Application
{
    public class PersonApplication : IPersonApplication
    {
        private readonly string cacheKey = "person";
        private readonly IDistributedCache _distributedCache;
        private readonly IPersonService _personService;

        public PersonApplication(IDistributedCache distributedCache, IPersonService personService)
        {
            _distributedCache = distributedCache;
            _personService = personService;
        }

        public async Task<T?> GetValueCacheAsync<T>(string key) where T : class
        {
            var cacheValue = await _distributedCache.GetStringAsync(key);
            if (cacheValue != null)
            {
                var value = JsonSerializer.Deserialize<T>(cacheValue);
                return value;
            }

            return null;
        }

        public async Task SetValueCacheAsync<T>(string key, T value) where T : class
        {
            var cacheOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(200));
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), cacheOptions);
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            var methodName = "GetByIdAsync";
            var key = $"{cacheKey}_{methodName}_{id}";

            var record = await GetValueCacheAsync<Person>(key);
            if (record != null)
                return record;

            record = await _personService.GetByIdAsync(id);
            await SetValueCacheAsync(key, record);
            return record;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            var methodName = "GetAllAsync";
            var key = $"{cacheKey}_{methodName}";

            var cacheValue = await GetValueCacheAsync<IEnumerable<Person>>(key);
            if (cacheValue != null)
                return cacheValue;

            var records = await _personService.GetAllAsync();
            await SetValueCacheAsync(key, records);
            return records;
        }

        public async Task<Person> CreateAsync()
        {
            var record = await _personService.CreateAsync();

            var methodName = "GetAllAsync";
            var key = $"{cacheKey}_{methodName}";
            await _distributedCache.RemoveAsync(key);

            return record;
        }

        public async Task<Person> UpdateAsync(int id)
        {
            var record = await _personService.UpdateAsync(id);

            var methodName = "GetAllAsync";
            var key = $"{cacheKey}_{methodName}";
            await _distributedCache.RemoveAsync(key);

            var methodName2 = "GetByIdAsync";
            var key2 = $"{cacheKey}_{methodName2}_{id}";
            await _distributedCache.RemoveAsync(key2);

            return record;
        }

        public async Task DeleteAsync(int id)
        {
            await _personService.DeleteAsync(id);

            

            var methodName = "GetAllAsync";
            var key = $"{cacheKey}_{methodName}";
            await _distributedCache.RemoveAsync(key);

            var methodName2 = "GetByIdAsync";
            var key2 = $"{cacheKey}_{methodName2}_{id}";
            await _distributedCache.RemoveAsync(key2);
        }
    }
}
