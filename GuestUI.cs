using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Guests;

namespace Hotel_Reservation_System
{
    // Static class so that an instance does not have to be created in order to use its functionality
    internal static class GuestUI
    {
        // Method returns a Guest instance once valid names have been provided
        public static Guest CreateGuest()
        {
            string firstName = GetValidName("Enter your first name: ");
            string lastName = GetValidName("Enter your last name: ");
            return new Guest(firstName, lastName);
        }

        // Method to get valid input from the user based on the regex rule defined
        public static string GetValidName(string prompt)       // Prompt variable used to display a unique message for getting the first name and last name
        {
            // Ensures that all chracters in the input string are characters from a-Z, spaces allowed
            // Adapted from https://www.programiz.com/csharp-programming/regex
            Regex regex = new Regex(@"^[A-Za-z]+(?:\s[A-Za-z]+)*$");
            string name;
            do
            {
                Console.Write(prompt);
                name = Console.ReadLine();
                if (!regex.IsMatch(name))
                {
                    Console.WriteLine("Invalid input. Please enter a valid name.");
                }
            }
            while (!regex.IsMatch(name));
            return name;
        }
    }
}