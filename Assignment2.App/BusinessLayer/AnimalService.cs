using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.App.BusinessLayer
{
    public class AnimalService
    {
        private readonly IDataStore store;

        public AnimalService(IDataStore store)
        {
            this.store = store;
        }

        public Animal AddAnimal()
        {
            return store.AddAnimal();
        }

        public IEnumerable<Animal> FindAnimals(int ownerId)
        {
            return store.FindAnimals(ownerId);
        }

        public void Save(string path)
        {
            store.Save(path);
        }
    }
}

