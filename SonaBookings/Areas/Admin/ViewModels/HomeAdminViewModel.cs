using SonaBookings.Areas.Identity.Data;
using SonaBookings.Models;

namespace SonaBookings.Areas.Admin.ViewModels
{
    public class HomeAdminViewModel
    {
        public IEnumerable<ApplicationUser>? Users { get; set; }

        public IEnumerable<Booking>? Bookings { get; set; }
        public IEnumerable<Room>? Rooms { get; set; }
        public IEnumerable<RoomType> RoomTypes { get; set; }
    }
}
