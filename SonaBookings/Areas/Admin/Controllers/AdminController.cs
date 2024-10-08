﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SonaBookings.Areas.Admin.ViewModels;
using SonaBookings.Areas.Identity.Data;

namespace SonaBookings.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize (Roles ="Admin")]
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
        public async Task<IActionResult> Index()
        {
            return View("Index", new HomeAdminViewModel
            {

                Users = await _userManager.Users.ToListAsync(),
                Rooms = await _context.Rooms.ToListAsync(),
                Bookings = await _context.Bookings.ToListAsync(),
                RoomTypes = await _context.RoomTypes.ToListAsync()
            });
        }

        [Route("Invoice")]
        public async Task<IActionResult> Invoice()
        {
            var invoice = _context.Invoices
                .Include(r => r.Booking);
            return View(await invoice.ToListAsync());
        }


    }
}
