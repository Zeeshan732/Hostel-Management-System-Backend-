using Microsoft.EntityFrameworkCore;

namespace Hostel_Management.Models
{
    public class HostelContext : DbContext
    {
        public HostelContext(DbContextOptions<HostelContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Student> Students { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Configure one-to-many relationship
        //    modelBuilder.Entity<Student>()
        //        .HasOne(s => s.Room)
        //        .WithMany(r => r.Students)
        //        .HasForeignKey(s => s.RoomId)
        //        .OnDelete(DeleteBehavior.SetNull); // Optional: Set RoomId to null when a room is deleted
        //}
    }
}
