using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rooms;
using Guests;
using Reservations;
using System.Reflection.Metadata;
using Hotel_Reservation_System;

namespace Hotels
{
    // Manages rooms and reservations.
    // Generate a hotel instance on opening the program
    // Generate a list of rooms 
    internal class Hotel
    {
        public List<Room> Rooms;
        public List<Room> SingleRooms;
        public List<Room> DoubleRooms;
        public List<Room> SuiteRooms;
        public List<Guest> Guests;
        public List<Reservation> Reservations;
        public List<Reservation> ReservationHistory;
        public const int MAXROOMSPERTYPE = 5;

        public Hotel()
        {
            Rooms = new List<Room>();
            SingleRooms = new List<Room>();
            DoubleRooms = new List<Room>();
            SuiteRooms = new List<Room>();
            Guests = new List<Guest>();
            Reservations = new List<Reservation>();
            ReservationHistory = new List<Reservation>();
            // Initialize rooms
            InitializeRooms(SingleRooms, "Single", 101);
            InitializeRooms(DoubleRooms, "Double", 201);
            InitializeRooms(SuiteRooms, "Suite", 301);
        }

        private void InitializeRooms(List<Room> roomList, string roomType, int startId)
        {
            for (int i = 0; i < MAXROOMSPERTYPE; i++)
            {
                Room r = new Room(startId + i, roomType, true);
                Rooms.Add(r);
                roomList.Add(r);
            }
        }

        public Guest getGuestFromName()
        {
            string first = "";
            string last = "";
            Console.WriteLine("Enter your details below.");
            first = GuestUI.GetValidName("Enter your first name: ");
            last = GuestUI.GetValidName("Enter your last name: ");

            foreach (Guest g in this.Guests)
            {
                if (first == g.FirstName && last == g.LastName)
                {
                    return g;
                }
            }
            return null;
        }

        public Room FindAvailableRoom(string roomType, DateTime checkIn, DateTime checkOut)
        {
            List<Room> rooms = getListOfRoomsByType(roomType);

            for (int i = 0; i < rooms.Count; i++)
            {
                Room room = rooms[i];
                bool isAvailable = true;

                for (int j = 0; j < Reservations.Count; j++)
                {
                    Reservation res = Reservations[j];

                    if (res.Room == room)
                    {
                        // Checks whether dates overlap
                        bool overlaps = (checkIn >= res.CheckInDate && checkIn < res.CheckOutDate) ||
                                        (checkOut > res.CheckInDate && checkOut <= res.CheckOutDate) ||
                                        (checkIn <= res.CheckInDate && checkOut >= res.CheckOutDate);

                        if (overlaps)
                        {
                            isAvailable = false;        // This room is not available for that time period
                            break;
                        }
                    }
                }

                if (isAvailable)
                {
                    return room;        // Return the first available room of the requested type within the requested time period
                }
            }
            return null; // No available room found
        }

        public void displayAvailableRoomsByDate(DateTime checkInDate, DateTime checkOutDate)
        {
            bool foundAvailableRoom = false;
            Console.WriteLine("Available Rooms:");
            for (int i = 0; i < Rooms.Count; i++)  // Loop through all rooms
            {
                Room room = Rooms[i];
                bool isAvailable = true;
                for (int j = 0; j < Reservations.Count; j++)  // Loop through reservations
                {
                    Reservation res = Reservations[j];

                    if (res.Room == room)
                    {
                        // Check if the requested period overlaps with an existing reservation
                        bool overlaps = (checkInDate >= res.CheckInDate && checkInDate < res.CheckOutDate) ||
                                        (checkOutDate > res.CheckInDate && checkOutDate <= res.CheckOutDate) ||
                                        (checkInDate <= res.CheckInDate && checkOutDate >= res.CheckOutDate);

                        if (overlaps)
                        {
                            isAvailable = false;
                            break; // No need to check further reservations for this room
                        }
                    }
                }

                if (isAvailable)
                {
                    foundAvailableRoom = true;
                    Console.WriteLine($"Room {room.RoomNumber} ({room.RoomType})");
                }
            }

            if (!foundAvailableRoom)
            {
                Console.WriteLine("No available rooms for the selected dates.");
            }
        }

        // Returns a List of Room instances that match the roomType parameter
        public List<Room> getListOfRoomsByType(string roomType)
        {
            if (roomType == "SINGLE")
            {
                return SingleRooms;
            }
            else if (roomType == "DOUBLE")
            {
                return DoubleRooms;
            }
            else
            {
                return SuiteRooms;
            }
        }

        public void MakeReservation(Guest guest, Room room, DateTime checkInDate, DateTime checkOutDate)
        {
            Reservation reservation = new Reservation(guest, room, checkInDate, checkOutDate);
            Reservations.Add(reservation);
            reservation.Guest.Reservations.Add(reservation);
            ReservationHistory.Add(reservation);
            room.IsAvailable = false;
            Console.WriteLine($"Room {room.RoomNumber} " +
                    $"booked for {guest.FirstName} {guest.LastName} " +
                    $"from {checkInDate.ToShortDateString()} " +
                    $"to {checkOutDate.ToShortDateString()}.");
        }

        public Reservation GetReservation(Guest guest, int roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            if (guest.Reservations.Count > 0)
            {
                foreach (Reservation res in guest.Reservations)
                {
                    if (res.Room.RoomNumber == roomNumber && res.CheckInDate == checkInDate && res.CheckOutDate == checkOutDate)
                    {
                        return res;
                    }
                }
            }
            else 
            {
                return null;
            }
            return null;
        }

        public bool HasRoomsAvailable()
        {
            if (Rooms.Count() >=1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DisplayReservationHistory()
        {
            Console.WriteLine("Reservation History:");
            foreach(Reservation res in ReservationHistory)
            {
                res.DisplayReservation();
            }
        }
    }
}