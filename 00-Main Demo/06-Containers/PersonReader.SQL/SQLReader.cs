using Microsoft.EntityFrameworkCore;
using PeopleViewer.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonReader.SQL
{
    public class SQLReader : IPersonReader
    {
        DbContextOptions<PersonContext> options;

        public SQLReader()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersonContext>();
            optionsBuilder.UseSqlite("Data Source=people.db");
            options = optionsBuilder.Options;
            
        }

        public Task<List<Person>> GetPeopleAsync()
        {
            using (var context = new PersonContext(options))
            {
                return context.People.ToListAsync();
            }
        }

        public Task<Person> GetPersonAsync(int id)
        {
            using (var context = new PersonContext(options))
            {
                return context.People.FirstOrDefaultAsync(p => p.Id == id);
            }
        }
    }
}
