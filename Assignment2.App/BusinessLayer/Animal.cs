using System.IO;

namespace Assignment2.App.BusinessLayer
{
    public class Animal
    {
        public string? Breed { get; set; }

        public int Id { get; set; }

        public string? Name { get; set; }

        public int OwnerId { get; set; }

        public string? Sex { get; set; }

        public string? Type { get; set; }
        //Removed the FromCsv and WriteToCsv methods from the Animal class as they are more related to storage than the model itself, and moved them to the CSVStore class to keep separation of concerns across the application
        public bool CheckIfValid()
        {
            return !(string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(Type)
                || string.IsNullOrEmpty(Sex)
                || (OwnerId == 0));
        }

        public override string ToString()
        {
            return $"{Name} [{Type}]";
        }
    }
}

       
