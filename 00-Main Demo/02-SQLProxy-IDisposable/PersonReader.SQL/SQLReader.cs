using Microsoft.EntityFrameworkCore;
using PeopleViewer.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonReader.SQL
{
    internal class SQLReader : IPersonReader, IDisposable
    {
        DbContextOptions<PersonContext> options;
        PersonContext context;

        public SQLReader()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersonContext>();
            optionsBuilder.UseSqlite("Data Source=people.db");
            options = optionsBuilder.Options;
            context = new PersonContext(options);
        }

        public Task<List<Person>> GetPeopleAsync()
        {
            return context.People.ToListAsync();
        }

        public Task<Person> GetPersonAsync(int id)
        {
            return context.People.FirstOrDefaultAsync(p => p.Id == id);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
