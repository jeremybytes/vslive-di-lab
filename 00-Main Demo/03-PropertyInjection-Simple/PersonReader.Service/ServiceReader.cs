using Newtonsoft.Json;
using PeopleViewer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PersonReader.Service
{
    public class ServiceReader : IPersonReader
    {
        HttpClient client = new HttpClient();

        public ServiceReader()
        {
            client.BaseAddress = new Uri("http://localhost:9874");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/people");
            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Person>>(stringResult);
            }
            return new List<Person>();
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync("api/people");
            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                var people = JsonConvert.DeserializeObject<List<Person>>(stringResult);
                return people?.FirstOrDefault(p => p.Id == id);
            }
            return null;
        }

        public async Task AddPersonAsync(Person newPerson)
        {
            string jsonPerson = JsonConvert.SerializeObject(newPerson);
            HttpResponseMessage response = await client.PostAsync("api/people",
                new StringContent(jsonPerson));
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdatePersonAsync(Person updatedPerson)
        {
            string jsonPerson = JsonConvert.SerializeObject(updatedPerson);
            HttpResponseMessage response = await client.PutAsync($"api/people/{updatedPerson.Id}",
                new StringContent(jsonPerson));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePersonAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"api/people/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
