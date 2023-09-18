using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet6TestCacheRedis.Domain;

namespace DotNet6TestCacheRedis.Repository
{
    public static class MemoryDatabase
    {
        public static List<Person> Persons { get; set; } = new List<Person>();
    }
}
