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
    }
}