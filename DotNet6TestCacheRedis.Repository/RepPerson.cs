using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet6TestCacheRedis.Domain;

namespace DotNet6TestCacheRedis.Repository
{
    public class RepPerson : IRepPerson
    {
        public async Task<Person?> GetByIdAsync(int id)
        {
            await DatabaseFakeDelay();
            return MemoryDatabase.Persons.FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            await DatabaseFakeDelay();
            return MemoryDatabase.Persons;
        }

        public async Task<Person> CreateAsync(Person value)
        {
            if (MemoryDatabase.Persons.Any())
                value.Id = MemoryDatabase.Persons.Max(p => p.Id) + 1;
            else
                value.Id = 1;

            MemoryDatabase.Persons.Add(value);
            return value;
        }

        public async Task<Person> UpdateAsync(Person value)
        {
            var updated = await GetByIdAsync(value.Id);
            if (updated == null)
                throw new Exception("Not foud");
            updated.Name = value.Name;
            return updated;
        }

        public async Task DeleteAsync(Person value)
        {
            await DatabaseFakeDelay();

            value.DeletedAt = DateTime.Now;
        }

        private async Task DatabaseFakeDelay()
        {
            await Task.Delay(5000);
        }
    }
}
