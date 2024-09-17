using SonaBookings.Models;

namespace SonaBookings.Areas.Admin.Interfaces
{
    public interface IRoomFactory
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int id);
        Task<IEnumerable<Capacity>> GetAllCapacitiesAsync();
        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();
        Task<IEnumerable<Size>> GetAllSizesAsync();
    }
}
