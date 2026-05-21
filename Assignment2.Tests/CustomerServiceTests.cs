using Assignment2.App.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Tests
{
    public class CustomerServiceTests
    {
        [Fact]
        public void AddCustomerUsesDataStore()
        {
            // Tests that CustomerService adds customers through IDataStore.
            // This is important because the service should depend on the abstraction, not CSVStore directly.
            IDataStore store = new CSVStore();
            var service = new CustomerService(store);

            var customer = service.AddCustomer();

            Assert.Contains(customer, store.Customers);
        }

        [Fact]
        public void FindCustomersReturnsMatchingCustomer()
        {
            // Tests that CustomerService can search customers by name.
            IDataStore store = new CSVStore();
            var service = new CustomerService(store);

            store.Customers.Add(new Customer { Id = 1, FirstName = "John", Surname = "Smith" });

            var results = service.FindCustomers("john").ToList();

            Assert.Single(results);
            Assert.Equal("John", results[0].FirstName);
        }
    }
}