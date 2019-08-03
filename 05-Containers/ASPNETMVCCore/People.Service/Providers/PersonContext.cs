using Microsoft.EntityFrameworkCore;
using People.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace People.Service.Providers
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options) 
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
    }
}
