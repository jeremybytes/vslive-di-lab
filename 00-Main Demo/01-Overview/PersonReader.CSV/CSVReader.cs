using PeopleViewer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonReader.CSV
{
    public class CSVReader : IPersonReader
    {
        public ICSVFileLoader FileLoader { get; set; }

        public CSVReader()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "People.txt";
            FileLoader = new CSVFileLoader(filePath);
        }

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
