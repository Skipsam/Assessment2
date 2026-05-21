using Assignment2.App.BusinessLayer;

namespace Assignment2.Tests
{
    public class AnimalTests
    {
        [Theory]
        [InlineData("Bob", "Dog", "Male", 1, true)]
        [InlineData(null, "Dog", "Male", 1, false)]
        [InlineData("Bob", null, "Male", 1, false)]
        [InlineData("Bob", "Dog", null, 1, false)]
        [InlineData("Bob", "Dog", "Male", 0, false)]
        public void CheckIfValidChecksProperties(string? name, string? type, string? sex, int owner, bool isValid)
        {
            // Arrange
            var animal = new Animal { Name = name, Type = type, Sex = sex, OwnerId = owner };

            // Act
            var result = animal.CheckIfValid();

            // Assert
            Assert.Equal(isValid, result);
        }

        [Fact]
        public void ToStringContainsCorrectDetails()
        {
            // Arrange
            var animal = new Animal
            {
                Name = "Bobby",
                Type = "Cat",
            };

            // Act
            var textValue = animal.ToString();

            // Assert
            Assert.Equal("Bobby [Cat]", textValue);
        }
        //Further tests were moved from AnimalTests.cs to CsvStoreTests.cs to better organize tests related to CSV functionality.

    }
}