using System.Threading.Tasks;

namespace PeopleViewer.Common
{
    public interface IPersonRepository : IPersonReader
    {
        Task AddPersonAsync(Person newPerson);
        Task UpdatePersonAsync(Person updatedPerson);
        Task DeletePersonAsync(int id);
    }
}
