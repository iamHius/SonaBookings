using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [Route("Index")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Rooms")]
        public async Task<IActionResult> Rooms()
        {
            var room = _context.Rooms
                .Include(r => r.Capacity)
                .Include(r => r.RoomType)
                .Include(r => r.Size);
            return View(await room.ToListAsync());
        }
        [Route("Bookings")]
        public async Task<IActionResult> Bookings()
        {
            var booking = _context.Bookings
                .Include(r => r.Room)
                .Include(r => r.User);
            return View(await booking.ToListAsync());
        } 
        
    }
}
