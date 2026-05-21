using System;
using System.IO;

namespace Assignment2.App.BusinessLayer
{
    public class Customer
    {
        public string? Address { get; set; }

        public string? FirstName { get; set; }

        public int Id { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Surname { get; set; }

        public bool CheckIfValid()
        {
            return !(string.IsNullOrEmpty(FirstName)
                || string.IsNullOrEmpty(PhoneNumber)
                || string.IsNullOrEmpty(Surname));
        }

        public override string ToString()
        {
            return $"{Surname}, {FirstName}".Trim(); ;
        }

        
    }
}