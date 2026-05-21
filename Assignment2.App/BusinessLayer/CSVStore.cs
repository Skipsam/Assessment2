using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assignment2.App.BusinessLayer
{
    // CSVStore is responsible for persisting application data using CSV files.
    // This class implements IDataStore so the rest of the application depends
    // on an abstraction rather than a concrete storage implementation.
    public class CSVStore : IDataStore
    {
        // Tracks the most recently assigned animal ID so new IDs remain unique.
        private int lastAnimalId = 0;
        // Tracks the most recently assigned customer ID so new IDs remain unique.
        private int lastCustomerId = 0;

        // Stores all animal records currently loaded into memory.
        public List<Animal> Animals { get; } = new();

        // Stores all customer records currently loaded into memory.
        public List<Customer> Customers { get; } = new();
        
        // Creates a new animal with a unique ID and adds it to the collection.
        public Animal AddAnimal()
        {
            var animal = new Animal
            {
                Id = ++lastAnimalId,
            };
            Animals.Add(animal);
            return animal;
        }

        // Creates a new customer with a unique ID and adds it to the collection.
        public Customer AddCustomer()
        {
            var customer = new Customer
            {
                Id = ++lastCustomerId,
            };

            Customers.Add(customer);
            return customer;
        }
        // Returns all animals belonging to a specific customer.
        public IEnumerable<Animal> FindAnimals(int ownerId)
        {
            var animals = this.Animals
                .Where(a => a.OwnerId == ownerId);
            return animals;
        }
        
        // Searches customers by first name or surname.
        public IEnumerable<Customer> FindCustomers(string name)
        {
            var customers = Customers
                .Where(c => c.FirstName?.Contains(name, System.StringComparison.InvariantCultureIgnoreCase) == true
                         || c.Surname?.Contains(name, System.StringComparison.InvariantCultureIgnoreCase) == true);
            return customers;
        }

        // Loads customer and animal data from CSV files.
        public void Load(string path)
        {
            var animalsPath = Path.Combine(path, "animals.csv");
            var customersPath = Path.Combine(path, "customers.csv");

            if (File.Exists(animalsPath)) LoadAnimals(animalsPath);
            if (File.Exists(customersPath)) LoadCustomers(customersPath);
        }

        // Saves customer and animal data into CSV files.
        public void Save(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var animalsPath = Path.Combine(path, "animals.csv");
            var customersPath = Path.Combine(path, "customers.csv");

            SaveAnimals(animalsPath);
            SaveCustomers(customersPath);
        }

        // Loads animal records from a CSV file into memory.
        private void LoadAnimals(string path)
        {
            Animals.Clear();
           
            using var stream = File.OpenRead(path);
            using var reader = new StreamReader(stream);
           
            var line = reader.ReadLine();
            line = reader.ReadLine();   // First line is a header - need to skip it
            
            while (!string.IsNullOrEmpty(line))
            {
                Animals.Add(AnimalFromCsv(line));
                line = reader.ReadLine();
            }
            // Updates the latest animal ID after loading data.
            lastAnimalId = Animals.Any() ? Animals.Max(a => a.Id) : 0;
        }

        // Loads customer records from a CSV file into memory.
        private void LoadCustomers(string path)
        {
            Customers.Clear();
            using var stream = File.OpenRead(path);
            using var reader = new StreamReader(stream);
            var line = reader.ReadLine();
            line = reader.ReadLine();   // First line is a header - need to skip it
            while (!string.IsNullOrEmpty(line))
            {
                Customers.Add(CustomerFromCsv(line));
                line = reader.ReadLine();
            }
            // Updates the latest customer ID after loading data.
            lastCustomerId = Customers.Any() ? Customers.Max(c => c.Id) : 0;
        }

        // Saves all animal records into a CSV file.
        private void SaveAnimals(string path)
        {
            using var stream = File.Create(path);
            using var writer = new StreamWriter(stream);
            WriteAnimalHeaderToCsv(writer);
            foreach (var animal in Animals)
            {
                WriteAnimalToCsv(writer, animal);
            }
        }

        // Saves all customer records into a CSV file.
        private void SaveCustomers(string path)
        {
            using var stream = File.Create(path);
            using var writer = new StreamWriter(stream);
            WriteCustomerHeaderToCsv(writer);
            foreach (var customer in Customers)
            {
                WriteCustomerToCsv(writer, customer);
            }
        }

        //Added methods for storing and retrieving data from CSV files instead of having seperated by model as likely to not require further models in the future,
        //but rather more likely to change storage method, helps keep seperation of concerns across application
        
        // Converts a CSV row into an Animal object.
        private Animal AnimalFromCsv(string line)
        {
            var parts = line.Split(',');

            return new Animal
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Type = parts[2],
                Breed = parts[3],
                Sex = parts[4],
                OwnerId = int.Parse(parts[5]),
            };
        }

        // Writes the CSV header row for animals.
        private void WriteAnimalHeaderToCsv(TextWriter writer)
        {
            writer.WriteLine("Id,Name,Type,Breed,Sex,OwnerId");
        }

        // Writes a single animal record into CSV format.
        private void WriteAnimalToCsv(TextWriter writer, Animal animal)
        {
            writer.WriteLine(string.Join(',', new[]
            {
        animal.Id.ToString(),
        animal.Name ?? string.Empty,
        animal.Type ?? string.Empty,
        animal.Breed ?? string.Empty,
        animal.Sex ?? string.Empty,
        animal.OwnerId.ToString(),
    }));
        }

        // Converts a CSV row into a Customer object.
        private Customer CustomerFromCsv(string line)
        {
            var parts = line.Split(',');

            return new Customer
            {
                Id = int.Parse(parts[0]),
                FirstName = parts[1],
                Surname = parts[2],
                PhoneNumber = parts[3],
                Address = parts[4][1..^1].Replace("\\n", Environment.NewLine),
            };
        }
        
        // Writes the CSV header row for customers.
        private void WriteCustomerHeaderToCsv(TextWriter writer)
        {
            writer.WriteLine("Id,First Name,Surname,Phone,Address");
        }

        // Writes a single customer record into CSV format.
        private void WriteCustomerToCsv(TextWriter writer, Customer customer)
        {
            var address = customer.Address ?? string.Empty;
            address = address.Replace("\n", "\\n");
            address = address.Replace("\r", "");

            writer.WriteLine(string.Join(',', new[]
            {
        customer.Id.ToString(),
        customer.FirstName ?? string.Empty,
        customer.Surname ?? string.Empty,
        customer.PhoneNumber ?? string.Empty,
        "\"" + address + "\"",
    }));
        }
    }
}