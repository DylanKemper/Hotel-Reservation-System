using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rooms
{
    // Individual room class, includes availability.
    internal class Room
    {
        public int RoomNumber;
        public string RoomType;
        public bool IsAvailable;

        public Room(int roomNumber, string type, bool isAvailable)
        {
            RoomNumber = roomNumber;
            RoomType = type;
            IsAvailable = isAvailable;
        }
    }
}