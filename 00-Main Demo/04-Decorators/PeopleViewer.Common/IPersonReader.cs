using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeopleViewer.Common
{
    public interface IPersonReader
    {
        Task<List<Person>> GetPeopleAsync();
        Task<Person> GetPersonAsync(int id);
    }
}