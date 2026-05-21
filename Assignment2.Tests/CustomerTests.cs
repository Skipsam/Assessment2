using Assignment2.App.BusinessLayer;

namespace Assignment2.Tests
{
    public class CustomerTests
    {
        [Theory]
        [InlineData("John", "Doe", "123-4567", true)]
        [InlineData(null, "Doe", "123-4567", false)]
        [InlineData("John", null, "123-4567", false)]
        [InlineData("John", "Doe", null, false)]
        public void CheckIfValidChecksProperties(string? firstName, string? surname, string? phoneNumber, bool isValid)
        {
            // Arrange
            var customer = new Customer { FirstName = firstName, Surname = surname, PhoneNumber = phoneNumber };

            // Act
            var result = customer.CheckIfValid();

            // Assert
            Assert.Equal(isValid, result);
        }

        [Fact]
        public void ToStringContainsCorrectDetails()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                Surname = "Doe",
            };

            // Act
            var textValue = customer.ToString();

            // Assert
            Assert.Equal("Doe, John", textValue);
        }
        [Fact]
        public void FromCsvCreatesCustomerCorrectly()
        {
            // Tests that a customer can be loaded correctly from CSV storage.
            //Arrange
            var line = "1,John,Doe,123-4567,\"1 Test Street\\nAuckland\"";

            //Act
            var customer = Customer.FromCsv(line);
            
            //Assert
            Assert.Equal(1, customer.Id);
            Assert.Equal("John", customer.FirstName);
            Assert.Equal("Doe", customer.Surname);
            Assert.Equal("123-4567", customer.PhoneNumber);
            Assert.Equal("1 Test Street" + Environment.NewLine + "Auckland", customer.Address);
        }

        [Fact]
        public void WriteHeaderToCsvWritesCorrectHeader()
        {
            // Tests that the CSV header matches the expected customer file format.
            //Arrange
            using var writer = new StringWriter();
            //Act
            Customer.WriteHeaderToCsv(writer);
            //Assert
            Assert.Equal("Id,First Name,Surname,Phone,Address" + Environment.NewLine, writer.ToString());
        }

        [Fact]
        public void WriteToCsvWritesCustomerCorrectly()
        {
            // Tests that customer data is saved in the correct CSV format.
            //Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                Surname = "Doe",
                PhoneNumber = "123-4567",
                Address = "1 Test Street"
            };

            using var writer = new StringWriter();
            
            //Act
            customer.WriteToCsv(writer);
           
            //Assert
            Assert.Equal("1,John,Doe,123-4567,\"1 Test Street\"" + Environment.NewLine, writer.ToString());
        }

        [Fact]
        public void WriteToCsvEscapesNewLinesInAddress()
        {
            // Tests that multi-line addresses are saved safely in one CSV row.
            // This is important because new lines could otherwise break CSV loading.
            //Arramge
            var customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                Surname = "Doe",
                PhoneNumber = "123-4567",
                Address = "Line 1\nLine 2"
            };

            using var writer = new StringWriter();
            
            //Act
            customer.WriteToCsv(writer);
            
            //Assert
            Assert.Equal("1,John,Doe,123-4567,\"Line 1\\nLine 2\"" + Environment.NewLine, writer.ToString());
        }
    }
}