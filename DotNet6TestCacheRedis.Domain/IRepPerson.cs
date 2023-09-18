using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6TestCacheRedis.Domain
{
    public interface IRepPerson
    {
        Task<Person?> GetByIdAsync(int id);
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> CreateAsync(Person value);
        Task<Person> UpdateAsync(Person value);
        Task DeleteAsync(Person value);
    }
}
