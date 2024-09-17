using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SonaBookings.Areas.Admin.Interfaces;
using SonaBookings.Areas.Identity.Data;
using SonaBookings.Models;

namespace SonaBookings.Areas.Admin.Factories
{
    [Area("Admin")]
    public class RoomFactory : IRoomFactory
    {
        private readonly ApplicationDbContext _context;

        public RoomFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms
                .Include(r => r.Capacity)
                .Include(r => r.RoomType)
                .Include(r => r.Size)
                .ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
#pragma warning disable CS8603 
            return await _context.Rooms
                .Include(r => r.Capacity)
                .Include(r => r.RoomType)
                .Include(r => r.Size)
                .FirstOrDefaultAsync(r => r.RoomId == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task AddRoomAsync(Room room)
        {
            _context.Rooms.Add(room);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoomAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Capacity>> GetAllCapacitiesAsync()
        {
            return await _context.Capacitys.ToListAsync();
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            return await _context.RoomTypes.ToListAsync();
        }

        public async Task<IEnumerable<Size>> GetAllSizesAsync()
        {
            return await _context.Sizes.ToListAsync();
        }
    }
}
