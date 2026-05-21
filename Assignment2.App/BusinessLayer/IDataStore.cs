using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.App.BusinessLayer
{
    public interface IDataStore
    {
        // IDataStore defines the operations required for storing and retrieving
        // application data. Using an interface allows the business layer to depend
        // on an abstraction rather than a specific storage implementation such as CSV.
        // This supports Inversion of Control and makes it easier to replace the
        // storage method later, for example with a database.

        List<Animal> Animals { get; }
        List<Customer> Customers { get; }

        Animal AddAnimal();
        Customer AddCustomer();

        IEnumerable<Animal> FindAnimals(int ownerId);
        IEnumerable<Customer> FindCustomers(string name);

        void Load(string path);
        void Save(string path);
    }
}
