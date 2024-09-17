using Microsoft.AspNetCore.Mvc;
using SonaBookings.Models;

namespace SonaBookings.Areas.Admin.Interfaces
{
    public interface IRoomFactory
    {
        [Area("Admin")]
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        [Area("Admin")]
        Task<Room> GetRoomByIdAsync(int id);
        [Area("Admin")]
        Task AddRoomAsync(Room room);
        [Area("Admin")]
        Task UpdateRoomAsync(Room room);
        [Area("Admin")]
        Task DeleteRoomAsync(int id);
        [Area("Admin")]
        Task<IEnumerable<Capacity>> GetAllCapacitiesAsync();
        [Area("Admin")]
        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();
        [Area("Admin")]
        Task<IEnumerable<Size>> GetAllSizesAsync();
    }
}
