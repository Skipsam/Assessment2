using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.App.BusinessLayer
{
    public class CustomerService
    {
        private readonly IDataStore store;

        public CustomerService(IDataStore store)
        {
            this.store = store;
        }
        public List<Customer> Customers => store.Customers;
        public Customer AddCustomer()
        {
            return store.AddCustomer();
        }

        public IEnumerable<Customer> FindCustomers(string name)
        {
            return store.FindCustomers(name);
        }

        public void Save(string path)
        {
            store.Save(path);
        }
    }
}