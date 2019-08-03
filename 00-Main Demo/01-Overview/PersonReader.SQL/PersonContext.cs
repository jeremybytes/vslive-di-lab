using Microsoft.EntityFrameworkCore;
using PeopleViewer.Common;

namespace PersonReader.SQL
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options)
            : base(options)
        { }

        public DbSet<Person> People { get; set; }
    }
}
