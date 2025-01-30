using Hostel_Management.Models;

namespace Hostel_Management.Interface
{
    public interface IRoomService
    {
        Task<(List<Room>, int)> GetAllRoomsFilteredAsync(int pageIndex, int pageSize, string? type, string? status);
        Task<Room?> GetRoomByRoomNumberAsync(int roomNumber);
        Task<bool> RoomNumberExists(int roomNumber);
        Task<Room> AddRoom(Room room);
        Task<Room?> UpdateRoom(int roomNumber, Room room);
        Task<Room?> DeleteRoom(int roomNumber);
        Task<Room?> ChangeRoomStatus(int roomNumber, string newStatus);
    }
}
