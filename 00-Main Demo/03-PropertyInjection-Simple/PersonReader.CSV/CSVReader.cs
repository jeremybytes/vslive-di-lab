using PeopleViewer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonReader.CSV
{
    public class CSVReader : IPersonReader
    {
        // START Code Block #1: "Simple" Property Injection
        // Dependency is instantiated by the constructor every time
        // (if the property is overridden before any method calls,
        //  the default is still instantiated even though it is never used).
        //public ICSVFileLoader FileLoader { get; set; }

        //public CSVReader()
        //{
        //    string filePath = AppDomain.CurrentDomain.BaseDirectory + "People.txt";
        //    FileLoader = new CSVFileLoader(filePath);
        //}
        // END Code Block #1

        // START Code Block #2: "Safe" Property Injection
        // Dependency is not instantiated until it is asked for
        // (if the property is overridden before any method calls,
        //  the default is never instantiated).
        private ICSVFileLoader fileLoader;
        public ICSVFileLoader FileLoader
        {
            get
            {
                if (fileLoader == null)
                {
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + "People.txt";
                    fileLoader = new CSVFileLoader(filePath);
                }
                return fileLoader;
            }
            set { fileLoader = value; }
        }

        // Note: No constructor since setup is done in the property getter.
        // END Code Block #2

        public Task<List<Person>> GetPeopleAsync()
        {
            string fileData = FileLoader.LoadFile();
            List<Person> people = ParseDataString(fileData);
            return Task.FromResult(people);
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            var people = await GetPeopleAsync();
            return people?.FirstOrDefault(p => p.Id == id);
        }

        private List<Person> ParseDataString(string csvData)
        {
            var people = new List<Person>();
            var lines = csvData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                try
                {
                    people.Add(ParsePersonString(line));
                }
                catch (Exception)
                {
                    // Skip the bad record, log it, and move to the next record
                    // log.Write($"Unable to parse record: {line}")
                }
            }
            return people;
        }

        private Person ParsePersonString(string personData)
        {
            var elements = personData.Split(',');
            var person = new Person()
            {
                Id = int.Parse(elements[0]),
                GivenName = elements[1],
                FamilyName = elements[2],
                StartDate = DateTime.Parse(elements[3]),
                Rating = int.Parse(elements[4]),
                FormatString = elements[5],
            };
            return person;
        }
    }
}
