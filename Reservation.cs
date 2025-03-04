using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guests;
using Rooms;

namespace Reservations
{
    // Stores Reservation information (Guest, Room, CheckIn and CheckOut dates)
    internal class Reservation
    {
        public Guest Guest;
        public Room Room;
        public DateTime CheckInDate;
        public DateTime CheckOutDate;
        public bool CheckedIn;

        public Reservation(Guest guest, Room room, DateTime checkInDate, DateTime checkOutDate)
        {
            Guest = guest;
            Room = room;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            CheckedIn = false;
        }

        public void DisplayReservation()
        {
            Console.WriteLine($"Reservation for {Guest.FirstName} {Guest.LastName}:");
            Console.WriteLine($"Room: {Room.RoomNumber} ({Room.RoomType})");
            Console.WriteLine($"Check-in: {CheckInDate.ToShortDateString()}");
            Console.WriteLine($"Check-out: {CheckOutDate.ToShortDateString()}\n");
        }
    }
}