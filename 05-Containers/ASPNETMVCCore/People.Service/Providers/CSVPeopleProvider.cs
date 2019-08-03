using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using People.Service.Models;

namespace People.Service.Providers
{
    public class CSVPeopleProvider : IPeopleProvider
    {
        string path;

        public CSVPeopleProvider()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "People.txt";
        }

        public List<Person> GetPeople()
        {
            var people = new List<Person>();

            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var elems = line.Split(',');
                        var per = new Person()
                        {
                            Id = Int32.Parse(elems[0]),
                            GivenName = elems[1],
                            FamilyName = elems[2],
                            StartDate = DateTime.Parse(elems[3]),
                            Rating = Int32.Parse(elems[4]),
                            FormatString = elems[5],
                        };
                        people.Add(per);
                    }
                }
            }
            return people;
        }

        public Person GetPerson(int id)
        {
            Person selPerson = new Person();
            if (File.Exists(path))
            {
                var sr = new StreamReader(path);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var elems = line.Split(',');
                    if (Int32.Parse(elems[0]) == id)
                    {
                        selPerson.Id = Int32.Parse(elems[0]);
                        selPerson.GivenName = elems[1];
                        selPerson.FamilyName = elems[2];
                        selPerson.StartDate = DateTime.Parse(elems[3]);
                        selPerson.Rating = Int32.Parse(elems[4]);
                        selPerson.FormatString = elems[5];
                    }
                }
            }

            return selPerson;
        }
    }
}
