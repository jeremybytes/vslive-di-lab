using PeopleViewer.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonReader.SQL
{
    public class SQLReaderProxy : IPersonReader
    {
        public Task<List<Person>> GetPeopleAsync()
        {
            using (var reader = new SQLReader())
            {
                return reader.GetPeopleAsync();
            }
        }

        public Task<Person> GetPersonAsync(int id)
        {
            using (var reader = new SQLReader())
            {
                return reader.GetPersonAsync(id);
            }
        }
    }
}
