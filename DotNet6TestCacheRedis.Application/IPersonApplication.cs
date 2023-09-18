using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet6TestCacheRedis.Domain;

namespace DotNet6TestCacheRedis.Application
{
    public interface IPersonApplication
    {
        Task<Person> GetByIdAsync(int id);
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> CreateAsync();
        Task<Person> UpdateAsync(int id);
        Task DeleteAsync(int id);
    }
}
