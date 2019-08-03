using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace People.Service
{
    public class PersonService : IPersonService
    {
        public List<Person> GetPeople()
        {
            Thread.Sleep(2000);
            var p = new List<Person>()
            {
                new Person() { Id=1, GivenName="John", FamilyName="Koenig",
                    StartDate = new DateTime(1975, 10, 17), Rating=6 },
                new Person() { Id=2, GivenName="Dylan", FamilyName="Hunt",
                    StartDate = new DateTime(2000, 10, 2), Rating=8 },
                new Person() { Id=3, GivenName="Leela", FamilyName="Turanga",
                    StartDate = new DateTime(1999, 3, 28), Rating=8,
                    FormatString = "{1} {0}" },
                new Person() { Id=4, GivenName="John", FamilyName="Crichton",
                    StartDate = new DateTime(1999, 3, 19), Rating=7 },
                new Person() { Id=5, GivenName="Dave", FamilyName="Lister",
                    StartDate = new DateTime(1988, 2, 15), Rating=9 },
                new Person() { Id=6, GivenName="Laura", FamilyName="Roslin",
                    StartDate = new DateTime(2003, 12, 8), Rating=6},
                new Person() { Id=7, GivenName="John", FamilyName="Sheridan",
                    StartDate = new DateTime(1994, 1, 26), Rating=6 },
                new Person() { Id=8, GivenName="Dante", FamilyName="Montana",
                    StartDate = new DateTime(2000, 11, 1), Rating=5 },
                new Person() { Id=9, GivenName="Isaac", FamilyName="Gampu",
                    StartDate = new DateTime(1977, 9, 10), Rating=4 },
            };
            return p;
        }

        public Person GetPerson(string familyName)
        {
            var p = new List<Person>()
            {
                new Person() { Id=1, GivenName="John", FamilyName="Koenig",
                    StartDate = new DateTime(1975, 10, 17), Rating=6 },
                new Person() { Id=2, GivenName="Dylan", FamilyName="Hunt",
                    StartDate = new DateTime(2000, 10, 2), Rating=8 },
                new Person() { Id=3, GivenName="Leela", FamilyName="Turanga",
                    StartDate = new DateTime(1999, 3, 28), Rating=8,
                    FormatString = "{1} {0}" },
                new Person() { Id=4, GivenName="John", FamilyName="Crichton",
                    StartDate = new DateTime(1999, 3, 19), Rating=7 },
                new Person() { Id=5, GivenName="Dave", FamilyName="Lister",
                    StartDate = new DateTime(1988, 2, 15), Rating=9 },
                new Person() { Id=6, GivenName="Laura", FamilyName="Roslin",
                    StartDate = new DateTime(2003, 12, 8), Rating=6},
                new Person() { Id=7, GivenName="John", FamilyName="Sheridan",
                    StartDate = new DateTime(1994, 1, 26), Rating=6 },
                new Person() { Id=8, GivenName="Dante", FamilyName="Montana",
                    StartDate = new DateTime(2000, 11, 1), Rating=5 },
                new Person() { Id=9, GivenName="Isaac", FamilyName="Gampu",
                    StartDate = new DateTime(1977, 9, 10), Rating=4 },
            };

            Person selPerson = p.Where(s => s.FamilyName == familyName).FirstOrDefault();
            return selPerson;
        }

        public void AddPerson(Person newPerson)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(string lastName, Person updatedPerson)
        {
            throw new NotImplementedException();
        }

        public void DeletePerson(string lastName)
        {
            throw new NotImplementedException();
        }

        public void UpdatePeople(List<Person> updatedPeople)
        {
            throw new NotImplementedException();
        }
    }
}
