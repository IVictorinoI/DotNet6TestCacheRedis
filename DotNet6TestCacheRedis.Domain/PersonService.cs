using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6TestCacheRedis.Domain
{
    public class PersonService : IPersonService
    {
        private readonly IRepPerson _repPerson;
        public PersonService(IRepPerson repPerson)
        {
            _repPerson = repPerson;
        }

        public Task<Person> GetByIdAsync(int id)
        {
            return _repPerson.GetByIdAsync(id);
        }

        public Task<IEnumerable<Person>> GetAllAsync()
        {
            return _repPerson.GetAllAsync();
        }

        public Task<Person> CreateAsync()
        {
            return _repPerson.CreateAsync(new Person()
            {
                Name = "Fulano - " + DateTime.Now,
                Date = DateTime.Now
            });
        }

        public async Task<Person> UpdateAsync(int id)
        {
            var record = await GetByIdAsync(id);
            if (record == null)
                throw new Exception("Not found");

            return await _repPerson.UpdateAsync(record);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await GetByIdAsync(id);
            if (record == null)
                throw new Exception("Not found");

            await _repPerson.DeleteAsync(record);
        }
    }
}
