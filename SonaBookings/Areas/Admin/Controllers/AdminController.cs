using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SonaBookings.Areas.Identity.Data;

namespace SonaBookings.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public AdminController(ApplicationDbContext context,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        [Route("Index")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        /*[Route("Rooms")]
        public async Task<IActionResult> Rooms()
        {
            var room = _context.Rooms
                .Include(r => r.Capacity)
                .Include(r => r.RoomType)
                .Include(r => r.Size);
            return View(await room.ToListAsync());
        }*/
        [Route("Bookings")]
        public async Task<IActionResult> Bookings()
        {
            var booking = _context.Bookings
                .Include(r => r.Room)
                .Include(r => r.User);
            return View(await booking.ToListAsync());
        }
        [Route("Invoice")]
        public async Task<IActionResult> Invoice()
        {
            var invoice = _context.Invoices
                .Include(r => r.Booking);
            return View(await invoice.ToListAsync());
        }

        [Route("Account")]
        public async Task<IActionResult> Account()
        {
            var user = _userManager.GetUserAsync(User);
            return View(await _userManager.Users.ToListAsync());

        }

    }
}
