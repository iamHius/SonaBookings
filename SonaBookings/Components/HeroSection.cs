using Microsoft.AspNetCore.Mvc;
using SonaBookings.Areas.Identity.Data;

namespace SonaBookings.Components
{
    public class HeroSection : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public HeroSection(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View("Index", _context.RoomTypes.ToList());
        }
    }
}
