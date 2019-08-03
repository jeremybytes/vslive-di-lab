using NUnit.Framework;
using PersonReader.CSV;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonReader.CSV.Tests
{
    public class CSVRepositoryTests
    {
        [Test]
        public async Task GetPeople_WithGoodRecords_ReturnsAllRecords()
        {
            var reader = new CSVReader();
            reader.FileLoader = new FakeFileLoader("Good");

            var result = await reader.GetPeopleAsync();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetPeople_WithNoFile_ThrowsFileNotFoundException()
        {
            var reader = new CSVReader();

            Assert.ThrowsAsync<FileNotFoundException>(
                () => reader.GetPeopleAsync());
        }

        [Test]
        public async Task GetPeople_WithSomeBadRecords_ReturnsGoodRecords()
        {
            var reader = new CSVReader();
            reader.FileLoader = new FakeFileLoader("Mixed");

            var result = await reader.GetPeopleAsync();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetPeople_WithOnlyBadRecords_ReturnsEmptyList()
        {
            var reader = new CSVReader();
            reader.FileLoader = new FakeFileLoader("Bad");

            var result = await reader.GetPeopleAsync();

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task GetPeople_WithEmptyFile_ReturnsEmptyList()
        {
            var reader = new CSVReader();
            reader.FileLoader = new FakeFileLoader("Empty");

            var result = await reader.GetPeopleAsync();

            Assert.AreEqual(0, result.Count());
        }
    }
}
