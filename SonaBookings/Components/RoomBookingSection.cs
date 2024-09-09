using Microsoft.AspNetCore.Mvc;
using SonaBookings.Areas.Identity.Data;

namespace SonaBookings.Components
{
    public class RoomBookingSection : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public RoomBookingSection(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View("Index", _context.Rooms.ToList());
        }
    }
}
