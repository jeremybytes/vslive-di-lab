using System.Collections.Generic;
using People.Service.Models;

namespace People.Service.Providers
{
    public interface IPeopleProvider
    {
        List<Person> GetPeople();
    }
}
