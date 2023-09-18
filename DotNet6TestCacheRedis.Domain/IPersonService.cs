using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6TestCacheRedis.Domain
{
    public interface IPersonService
    {
        Task<Person> GetByIdAsync(int id);
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> CreateAsync();
        Task<Person> UpdateAsync(int id);
        Task DeleteAsync(int id);
    }
}
