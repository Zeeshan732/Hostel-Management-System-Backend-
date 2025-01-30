using Hostel_Management.Interface;
using Hostel_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hostel_Management.Services
{
    public class RoomService : IRoomService
    {
        private readonly HostelContext _context;

        public RoomService(HostelContext context)
        {
            _context = context;
        }

        public async Task<(List<Room>, int)> GetAllRoomsFilteredAsync(int pageIndex, int pageSize,string? type, string? status)
        {
            var query = _context.Rooms.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(type))
                query = query.Where(r => r.Type == type);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);

            // Get total count for pagination
            int totalCount = await query.CountAsync();

            // Apply pagination
            var paginatedRooms = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedRooms, totalCount);
        }
        


        public async Task<Room?> GetRoomByRoomNumberAsync(int roomNumber)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
        }


        public async Task<bool> RoomNumberExists(int roomNumber)
        {
            return await _context.Rooms.AnyAsync(r => r.RoomNumber == roomNumber);
        }

        public async Task<Room> AddRoom(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room?> UpdateRoom(int roomNumber, Room room)
        {
            var existingRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);

            if (existingRoom == null)
                return null;

            // Update fields
            existingRoom.TotalBeds = room.TotalBeds;
            existingRoom.OccupiedBeds = room.OccupiedBeds;
            existingRoom.RoomPrice = room.RoomPrice; 
            existingRoom.Type = room.Type;  

            // Dynamically recalculate status and remaining beds
            UpdateRoomStatus(existingRoom);

            // Save changes
            _context.Entry(existingRoom).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingRoom;
        }


        private void UpdateRoomStatus(Room room)
        {
            // Recalculate RemainingBeds
            room.RemainingBeds = room.TotalBeds - room.OccupiedBeds;

            // Update Status based on RemainingBeds
            if (room.RemainingBeds == 0)
            {
                room.Status = "Occupied"; // All beds are occupied
            }
            else if (room.OccupiedBeds == 0)
            {
                room.Status = "Available"; // No beds are occupied
            }
            else
            {
                room.Status = "Partially Available"; // Some beds are occupied
            }
        }



        public async Task<Room?> DeleteRoom(int roomNumber)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);

            if (room == null || room.Status == "Occupied" || room.OccupiedBeds > 0)
                return null;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return room;
        }

        public async Task<Room?> ChangeRoomStatus(int roomNumber, string newStatus)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);

            if (room == null)
                return null;

            // Handling logic for Double rooms
            if (room.Type == "Double")
            {
                if (newStatus == "Occupied")
                {
                    room.OccupiedBeds++;

                    // Check if all beds are occupied
                    if (room.OccupiedBeds == room.TotalBeds)
                    {
                        room.Status = "Occupied"; // All beds occupied
                    }
                    else
                    {
                        room.Status = "Partially Available"; // Some beds occupied
                    }
                }
                else if (newStatus == "Available")
                {
                    room.OccupiedBeds--;

                    // Check if no beds are occupied
                    if (room.OccupiedBeds == 0)
                    {
                        room.Status = "Available"; // All beds are free
                    }
                    else
                    {
                        room.Status = "Partially Available"; // Some beds occupied
                    }
                }
            }
            else // For non-double rooms, status is directly set
            {
                room.Status = newStatus;
            }

            // Update the RemainingBeds dynamically
            room.RemainingBeds = room.TotalBeds - room.OccupiedBeds;

            // If no remaining beds, status is "Occupied"
            if (room.RemainingBeds == 0)
            {
                room.Status = "Occupied";
            }

            // Mark the room as modified and save changes
            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return room;
        }

        
    }
}
