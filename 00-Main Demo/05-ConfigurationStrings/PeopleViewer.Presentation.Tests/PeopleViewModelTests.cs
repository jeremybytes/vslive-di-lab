using NUnit.Framework;
using PeopleViewer.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleViewer.Presentation.Tests
{
    public class PeopleViewModelTests
    {
        private IPersonReader GetTestRepository()
        {
            return new FakeRepository();
        }

        [Test]
        public async Task RefreshPeople_OnExecute_PeopleIsPopulated()
        {
            // Arrange
            var repository = GetTestRepository();
            var vm = new PeopleViewModel(repository);

            // Act
            await vm.RefreshPeople();

            // Assert
            Assert.IsNotNull(vm.People);
            Assert.AreEqual(2, vm.People.Count());
        }

        [Test]
        public async Task ClearPeople_OnExecute_PeopleIsEmpty()
        {
            // Arrange
            var repository = GetTestRepository();
            var vm = new PeopleViewModel(repository);
            await vm.RefreshPeople();
            Assert.AreEqual(2, vm.People.Count(), "Invalid Arrangement");

            // Act
            vm.ClearPeople();

            // Assert
            Assert.AreEqual(0, vm.People.Count());
        }
    }
}
