using Microsoft.EntityFrameworkCore;
using People.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace People.Service.Providers
{
    public class SQLPeopleProvider : IPeopleProvider
    {
        DbContextOptions<PersonContext> options;

        public SQLPeopleProvider()
        {
            string source = AppDomain.CurrentDomain.BaseDirectory + "People.db";
            var optionsBuilder = new DbContextOptionsBuilder<PersonContext>();
            optionsBuilder.UseSqlite($"Data Source={source}");
            options = optionsBuilder.Options;
        }

        public List<Person> GetPeople()
        {
            using (var context = new PersonContext(options))
            {
                return context.People.ToList();
            }
        } 

        public Person GetPerson(int id)
        {
            using (var context = new PersonContext(options))
            {
                return context.People.Where(p => p.Id == id).FirstOrDefault();
            }
        }
    }
}
