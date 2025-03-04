using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Reservations;
using Hotel_Reservation_System;
using Hotels;

namespace Guests
{
    // For Guest information. Keeps track of Guest reservations.
    internal class Guest
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public List<Reservation> Reservations;       // List of all bookings made by user.

        public Guest(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Reservations = new List<Reservation>();
        }
    }
}