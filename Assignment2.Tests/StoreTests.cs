using Assignment2.App.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Tests
{
    public class StoreTests
    {
        [Fact]
        public void AddAnimalCreatesAnimalWithNewId()
        {
            // Tests that AddAnimal creates a new animal with an automatically assigned ID.
            // This is important because every animal record must have a unique identifier.
            var store = new Store();

            var animal = store.AddAnimal();

            Assert.Equal(1, animal.Id);
        }

        [Fact]
        public void AddCustomerCreatesCustomerWithNewId()
        {
            // Tests that AddCustomer creates a new customer with an automatically assigned ID.
            // This is important because every customer record must have a unique identifier.
            var store = new Store();

            var customer = store.AddCustomer();

            Assert.Equal(1, customer.Id);
        }

        [Fact]
        public void FindAnimalsReturnsAnimalsForOwnerOnly()
        {
            // Tests that FindAnimals only returns animals belonging to the selected customer.
            // This is important because the UI should not show another customer's animals.
            var store = new Store();

            store.Animals.Add(new Animal { Id = 1, Name = "Dog A", OwnerId = 10 });
            store.Animals.Add(new Animal { Id = 2, Name = "Cat B", OwnerId = 20 });
            store.Animals.Add(new Animal { Id = 3, Name = "Dog C", OwnerId = 10 });

            var results = store.FindAnimals(10).ToList();

            Assert.Equal(2, results.Count);
            Assert.All(results, animal => Assert.Equal(10, animal.OwnerId));
        }

        [Fact]
        public void FindCustomersReturnsMatchingFirstName()
        {
            // Tests that customers can be searched by first name.
            // This is important because the current GUI includes customer searching.
            var store = new Store();

            store.Customers.Add(new Customer { Id = 1, FirstName = "John", Surname = "Smith" });
            store.Customers.Add(new Customer { Id = 2, FirstName = "Mary", Surname = "Jones" });

            var results = store.FindCustomers("john").ToList();

            Assert.Single(results);
            Assert.Equal("John", results[0].FirstName);
        }

        [Fact]
        public void FindCustomersReturnsMatchingSurname()
        {
            // Tests that customers can be searched by surname.
            // This is important because users may search by family name instead of first name.
            var store = new Store();

            store.Customers.Add(new Customer { Id = 1, FirstName = "John", Surname = "Smith" });
            store.Customers.Add(new Customer { Id = 2, FirstName = "Mary", Surname = "Jones" });

            var results = store.FindCustomers("smith").ToList();

            Assert.Single(results);
            Assert.Equal("Smith", results[0].Surname);
        }

        [Fact]
        public void SaveCreatesCsvFiles()
        {
            // Tests that Save creates both CSV files.
            // This is important because the current system relies on local file persistence.
            var store = new Store();
            var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            store.Customers.Add(new Customer
            {
                Id = 1,
                FirstName = "John",
                Surname = "Doe",
                PhoneNumber = "123"
            });

            store.Animals.Add(new Animal
            {
                Id = 1,
                Name = "Bobby",
                Type = "Dog",
                Sex = "Male",
                OwnerId = 1
            });

            store.Save(folder);

            Assert.True(File.Exists(Path.Combine(folder, "customers.csv")));
            Assert.True(File.Exists(Path.Combine(folder, "animals.csv")));

            Directory.Delete(folder, true);
        }

        [Fact]
        public void LoadReadsCustomersAndAnimalsFromCsvFiles()
        {
            // Tests that Load correctly reads existing customer and animal CSV files.
            // This is important so existing saved clinic data is not lost after refactoring.
            var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(folder);

            File.WriteAllText(Path.Combine(folder, "customers.csv"),
                "Id,First Name,Surname,Phone,Address" + Environment.NewLine +
                "1,John,Doe,123,\"Test Address\"" + Environment.NewLine);

            File.WriteAllText(Path.Combine(folder, "animals.csv"),
                "Id,Name,Type,Breed,Sex,OwnerId" + Environment.NewLine +
                "1,Bobby,Dog,Labrador,Male,1" + Environment.NewLine);

            var store = new Store();

            store.Load(folder);

            Assert.Single(store.Customers);
            Assert.Single(store.Animals);
            Assert.Equal("John", store.Customers[0].FirstName);
            Assert.Equal("Bobby", store.Animals[0].Name);

            Directory.Delete(folder, true);
        }

        [Fact]
        public void SaveThenLoadPreservesCustomerAndAnimalData()
        {
            // Tests that data saved by Store can be loaded again correctly.
            // This is important because it checks the full persistence workflow.
            var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            var originalStore = new Store();

            originalStore.Customers.Add(new Customer
            {
                Id = 1,
                FirstName = "John",
                Surname = "Doe",
                PhoneNumber = "123",
                Address = "Test Address"
            });

            originalStore.Animals.Add(new Animal
            {
                Id = 1,
                Name = "Bobby",
                Type = "Dog",
                Breed = "Labrador",
                Sex = "Male",
                OwnerId = 1
            });

            originalStore.Save(folder);

            var loadedStore = new Store();
            loadedStore.Load(folder);

            Assert.Single(loadedStore.Customers);
            Assert.Single(loadedStore.Animals);
            Assert.Equal("John", loadedStore.Customers[0].FirstName);
            Assert.Equal("Bobby", loadedStore.Animals[0].Name);

            Directory.Delete(folder, true);
        }
    }
}