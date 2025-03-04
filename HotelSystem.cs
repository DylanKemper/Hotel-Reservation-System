using Guests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotels;
using Rooms;
using Reservations;

namespace Hotel_Reservation_System
{
    internal class HotelSystem
    {
        private Hotel Hotel;

        // Constructor
        public HotelSystem()
        {
            Hotel = new Hotel();
        }

        // The main body of code with all logic
        public void Run()
        {
            bool exitInvoked = false;
            int userChoice = 0;
            Guest guest = null;

            // Keep looping until the user wants to exit the program
            do
            {
                userChoice = GetChoice();       // GetChoice() handles data validation
                if (userChoice == 6)
                {
                    exitInvoked = true;         // Check this first to save time
                }
                switch (userChoice)
                {
                    case 1: // Book Room
                        {
                            if (!Hotel.HasRoomsAvailable())
                            {
                                Console.Clear();
                                Console.WriteLine($"No available rooms. Please try again later.");
                                break;
                            }

                            if (Hotel.Guests.Count() > 0)       // If a guest has been created, then a room has been booked
                            {
                                Console.WriteLine("Have you made a reservation with us before? (y/n)");     // To check if a guest instance exists for this user
                                string response = Console.ReadLine().ToLower();
                                if (response == "y")
                                {
                                    guest = Hotel.getGuestFromName();       // Return guest instance from their first and last names. Input validation done here
                                    if (guest == null)                      // If returned guest is null, then no guest instance ws found that matches the first and last names entered
                                    {
                                        Console.WriteLine("Guest not found.");
                                        break;
                                    }
                                    Console.WriteLine($"Welcome back, {guest.FirstName} {guest.LastName}");
                                }
                                else if (response == "n")
                                {
                                    Console.WriteLine("Welcome! Let's create a profile for you.");
                                    guest = GuestUI.CreateGuest();          // GuestUI handles all data validation for Guest instance creation
                                    Hotel.Guests.Add(guest);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input.");    // If reponse is not "y" or "n", then it is not a valid response
                                    break;
                                }
                            }
                            else
                            {
                                guest = GuestUI.CreateGuest();
                                Hotel.Guests.Add(guest);
                            }

                            bool validRoomType = false;
                            string roomType = "";
                            Console.WriteLine("Enter room type (Single/Double/Suite): ");
                            // Allow user to select their preferred room type
                            do
                            {
                                roomType = Console.ReadLine().ToUpper();
                                switch (roomType)                   // Checks whether the room type entered is valid
                                {
                                    case "SINGLE":
                                    case "DOUBLE":
                                    case "SUITE":
                                        validRoomType = true;
                                        break;
                                    default:
                                        Console.WriteLine("Please enter a valid room type (Single/Double/Suite):");
                                        break;
                                }
                            }
                            while (!validRoomType);

                            // Get valid checkin and checkout dates
                            DateTime checkInDate, checkOutDate;
                            do
                            {
                                Console.Write("Enter check-in date (MM/DD/YYYY): ");
                            }
                            while (!TryGetValidDate(out checkInDate));          // TryGetValidDate includes data validation
                            do
                            {
                                Console.Write("Enter check-out date (MM/DD/YYYY): ");
                            }
                            while (!TryGetValidDate(out checkOutDate) || checkOutDate <= checkInDate);

                            // Create room instance is a matching room is available
                            Room room = Hotel.FindAvailableRoom(roomType, checkInDate, checkOutDate);
                            if (room == null)
                            {
                                Console.WriteLine($"No {roomType} rooms are available in that period. Please choose another room type or reservation date.");
                                break;
                            }

                            Console.Clear();
                            Hotel.MakeReservation(guest, room, checkInDate, checkOutDate);
                            break;
                        }

                    case 2: // Check-in Guest
                        {
                            Console.Clear();
                            if (Hotel.Reservations.Count() > 0)
                            {
                                Reservation reservation = GetValidReservation(Hotel);
                                if (reservation != null)
                                {
                                    if (reservation.CheckedIn)
                                    {
                                        Console.WriteLine("You have already checked into this room.");
                                    }
                                    else
                                    {
                                        reservation.CheckedIn = true;
                                        Console.WriteLine("You have checked in.");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("No guests have reserved any rooms yet.");
                            }
                            break;
                        }

                    case 3: // Check-out Guest
                        {
                            Console.Clear();
                            if (Hotel.Reservations.Count() > 0)
                            {
                                Reservation reservation = GetValidReservation(Hotel);
                                if (reservation != null && reservation.CheckedIn)
                                {
                                    reservation.CheckedIn = false;
                                    reservation.Guest.Reservations.Remove(reservation);         // Once a room has been checked out, remove that reservation from the Guest and Hotel classes
                                    Hotel.Reservations.Remove(reservation);
                                    Console.WriteLine("You have checked out.");
                                }
                                else
                                {
                                    Console.WriteLine("Either no matching reservation was found or the guest has not checked in yet.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No guests have reserved any rooms yet.");
                            }
                            break;
                        }

                    case 4: // View available Rooms
                        {
                            Console.Clear();
                            DateTime checkInDate, checkOutDate;

                            do
                            {
                                Console.Write("Enter check-in date (MM/DD/YYYY): ");
                            }
                            while (!TryGetValidDate(out checkInDate));

                            do
                            {
                                Console.Write("Enter check-out date (MM/DD/YYYY): ");
                            }
                            while (!TryGetValidDate(out checkOutDate) || checkOutDate <= checkInDate);

                            Hotel.displayAvailableRoomsByDate(checkInDate, checkOutDate);
                            break;
                        }

                    case 5: // View Reservation History
                        {
                            Console.Clear();
                            if (Hotel.ReservationHistory.Count() > 0)
                            {
                                Hotel.DisplayReservationHistory();
                            }
                            else
                            {
                                Console.WriteLine("No reservations have been made yet.");
                            }
                            break;
                        }
                }
            }
            while (!exitInvoked);
        }

        // Used to return an integer value for a switch-case loop. Includes data validation
        private int GetChoice()
        {
            int userChoice = 0;
            bool validChoice = false;

            do
            {
                Console.WriteLine("Welcome to GrandStay Hotel's Reservation Management System!");
                Console.WriteLine("1. Book Room");
                Console.WriteLine("2. Check-in Guest");
                Console.WriteLine("3. Check-out Guest");
                Console.WriteLine("4. View Available Rooms");
                Console.WriteLine("5. View Reservation History");
                Console.WriteLine("6. Exit");

                do
                {
                    Console.Write("Choose an option: ");
                    string strChoice = Console.ReadLine();

                    if (int.TryParse(strChoice, out userChoice) && userChoice >= 1 && userChoice <= 6)
                    {
                        validChoice = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Enter a number (1-6).");
                    }
                }
                while (!validChoice);
            }
            while (false);

            return userChoice;
        }

        // Returns a Reservation instance if the reservation matches a Reservation instance in the Hotel class' List of Reservations
        private static Reservation GetValidReservation(Hotel hotel)
        {
            Guest guest = hotel.getGuestFromName();
            if (guest == null)
            {
                Console.WriteLine("No guest by this name exists.");
                return null;
            }

            int roomNum;
            Console.Write("Enter Room Number: ");
            while (!int.TryParse(Console.ReadLine(), out roomNum))
            {
                Console.WriteLine("Invalid room number. Please enter a valid number:");
            }

            DateTime checkInDate, checkOutDate;

            do
            {
                Console.Write("Enter check-in date (MM/DD/YYYY): ");
            }
            while (!TryGetValidDate(out checkInDate));

            do
            {
                Console.Write("Enter check-out date (MM/DD/YYYY): ");
            }
            while (!TryGetValidDate(out checkOutDate) || checkOutDate <= checkInDate);

            Reservation reservation = hotel.GetReservation(guest, roomNum, checkInDate, checkOutDate);

            if (reservation == null)
            {
                Console.WriteLine("No reservation like this exists. Please try again.");
            }

            return reservation;
        }

        // Data validation for DateTime instances
        private static bool TryGetValidDate(out DateTime date)
        {
            string input = Console.ReadLine();
            bool isValidDate = DateTime.TryParse(input, out date);

            if (!isValidDate)
            {
                Console.WriteLine("Invalid date format. Please enter a valid date (MM/DD/YYYY): ");
            }

            return isValidDate;
        }
    }
}
