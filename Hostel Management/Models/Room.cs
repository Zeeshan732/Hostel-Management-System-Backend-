namespace Hostel_Management.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }

        public decimal RoomPrice { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int TotalBeds { get; set; } 
        public int OccupiedBeds { get; set; }
        public int RemainingBeds { get; set; }

        //public ICollection<Student> Students { get; set; }
    }
}
