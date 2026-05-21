using Assignment2.App.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Tests
{
    public class AnimalServiceTests
    {
        [Fact]
        public void AddAnimalUsesDataStore()
        {
            // Tests that AnimalService adds animals through IDataStore.
            // This is important because the service should depend on the abstraction, not CSVStore directly.
            IDataStore store = new CSVStore();
            var service = new AnimalService(store);

            var animal = service.AddAnimal();

            Assert.Contains(animal, store.Animals);
        }

        [Fact]
        public void FindAnimalsReturnsAnimalsForOwner()
        {
            // Tests that AnimalService can retrieve animals for a selected owner.
            IDataStore store = new CSVStore();
            var service = new AnimalService(store);

            store.Animals.Add(new Animal { Id = 1, Name = "Dog", OwnerId = 1 });
            store.Animals.Add(new Animal { Id = 2, Name = "Cat", OwnerId = 2 });

            var results = service.FindAnimals(1).ToList();

            Assert.Single(results);
            Assert.Equal(1, results[0].OwnerId);
        }
    }
}
