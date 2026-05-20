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
        
        [Fact]
        public void FromCsvCreatesAnimalCorrectly()
        {
            // Tests that an animal can be loaded correctly from CSV storage.
            // This is important because the current system stores data in CSV files.
            //Arrange
            var line = "1,Bobby,Cat,Siamese,Male,2";

            //Act
            var animal = Animal.FromCsv(line);

            //Assert
            Assert.Equal(1, animal.Id);
            Assert.Equal("Bobby", animal.Name);
            Assert.Equal("Cat", animal.Type);
            Assert.Equal("Siamese", animal.Breed);
            Assert.Equal("Male", animal.Sex);
            Assert.Equal(2, animal.OwnerId);
        }
        
        [Fact]
        public void WriteHeaderToCsvWritesCorrectHeader()
        {
            // Tests that the CSV header matches the expected animal file format.
            using var writer = new StringWriter();

            //Act
            Animal.WriteHeaderToCsv(writer);

            //Assert
            Assert.Equal("Id,Name,Type,Breed,Sex,OwnerId" + Environment.NewLine, writer.ToString());
        }
        [Fact]
        public void WriteToCsvWritesAnimalCorrectly()
        {
            // Tests that animal data is saved in the correct CSV format.
            //Arrange
            var animal = new Animal
            {
                Id = 1,
                Name = "Bobby",
                Type = "Cat",
                Breed = "Siamese",
                Sex = "Male",
                OwnerId = 2
            };

            using var writer = new StringWriter();
            
            //Act
            animal.WriteToCsv(writer);

            //Assert
            Assert.Equal("1,Bobby,Cat,Siamese,Male,2" + Environment.NewLine, writer.ToString());
        }


    }
}