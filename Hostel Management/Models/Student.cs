namespace Hostel_Management.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string ContactInfo { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        // Foreign key for Room
        public int? RoomId { get; set; }

        // Navigation property for Room
        public Room Room { get; set; }
    }
}
