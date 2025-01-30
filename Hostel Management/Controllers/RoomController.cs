using FluentValidation;
using Hostel_Management.Interface;
using Hostel_Management.Models;
using Hostel_Management.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hostel_Management.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IValidator<Room> _roomValidator;

        public RoomController(IRoomService roomService, IValidator<Room> roomValidator)
        {
            _roomService = roomService;
            _roomValidator = roomValidator;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllRooms(int pageIndex, int pageSize, string? type, string? status)
        {
            var (rooms, totalCounts) = await _roomService.GetAllRoomsFilteredAsync(pageIndex, pageSize, type, status);

            
            var response = new APIResponse
            {
                IsSuccessful = true,
                Message = "Rooms fetched successfully.",
                Data = new { Rooms = rooms, TotalCounts = totalCounts }

            };

            return Ok(response);
        }

        [HttpGet("{roomNumber}")]
        public async Task<ActionResult<APIResponse>> GetRoomByRoomNumber(int roomNumber)
        {
            var room = await _roomService.GetRoomByRoomNumberAsync(roomNumber);

            if (room == null)
                return NotFound(new APIResponse { IsSuccessful = false, Message = "Room not found." });

            return Ok(new APIResponse
            {
                IsSuccessful = true,
                Message = "Room fetched successfully.",
                Data = room
            });
        }

        [HttpPost("rooms")]
        public async Task<ActionResult<APIResponse>> AddRoom([FromBody] Room room)
        {
            var validationResult = await _roomValidator.ValidateAsync(room);
            if (!validationResult.IsValid)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccessful = false,
                    Message = "Validation failed.",
                    Data = validationResult.Errors
                });
            }

            if (await _roomService.RoomNumberExists(room.RoomNumber))
                return BadRequest(new APIResponse { IsSuccessful = false, Message = "Room number already exists." });

            var addedRoom = await _roomService.AddRoom(room);
            return CreatedAtAction(nameof(GetAllRooms), new { id = addedRoom.RoomId }, new APIResponse
            {
                IsSuccessful = true,
                Message = "Room added successfully.",
                Data = addedRoom
            });
        }

        [HttpPut("{roomNumber}")]
        public async Task<ActionResult<APIResponse>> UpdateRoom(int roomNumber, [FromBody] Room room)
        {
            var validationResult = await _roomValidator.ValidateAsync(room);
            if (!validationResult.IsValid)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccessful = false,
                    Message = "Validation failed.",
                    Data = validationResult.Errors
                });
            }

            var updatedRoom = await _roomService.UpdateRoom(roomNumber, room);

            if (updatedRoom == null)
                return NotFound(new APIResponse { IsSuccessful = false, Message = "Room not found." });

            return Ok(new APIResponse
            {
                IsSuccessful = true,
                Message = "Room updated successfully.",
                Data = updatedRoom
            });
        }

        [HttpDelete("{roomNumber}")]
        public async Task<ActionResult<APIResponse>> DeleteRoom(int roomNumber)
        {
            var deletedRoom = await _roomService.DeleteRoom(roomNumber);

            if (deletedRoom == null)
                return BadRequest(new APIResponse { IsSuccessful = false, Message = "Cannot delete a room that is occupied or does not exist." });

            return Ok(new APIResponse
            {
                IsSuccessful = true,
                Message = "Room deleted successfully.",
                Data = deletedRoom
            });
        }

        [HttpPatch("{roomNumber}/status")]
        public async Task<ActionResult<APIResponse>> ChangeRoomStatus(int roomNumber, [FromBody] string newStatus)
        {
            var updatedRoom = await _roomService.ChangeRoomStatus(roomNumber, newStatus);

            if (updatedRoom == null)
                return NotFound(new APIResponse { IsSuccessful = false, Message = "Room not found." });

            return Ok(new APIResponse
            {
                IsSuccessful = true,
                Message = "Room status updated successfully.",
                Data = updatedRoom
            });
        }
    }
}
