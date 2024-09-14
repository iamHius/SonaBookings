using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SonaBookings.Areas.Identity.Data;
using SonaBookings.Models;
using SonaBookings.Models.ViewModels;


namespace SonaBookings.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var currentUser = _userManager.GetUserId(User);
            var booking = await _context.Bookings.Include(b => b.Room)
                .Where(b => b.UserId == currentUser)
                .ToListAsync();
            return View(booking);
        }

        public async Task<IActionResult> ViewAllInvoice()
        {
            var currentUser = _userManager.GetUserId(User);
            var invoice = await _context.Invoices
                .Include(b => b.Booking)
                .Where(b => b.Booking.UserId == currentUser)
                .ToListAsync();
            return View(invoice);
        }

        [ActionName("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if(booking == null)
            {
                return NotFound();
            }
            int totalDays = (booking.CheckOutDate - booking.CheckInDate).Value.Days;
            if (totalDays <= 0)
            {
                totalDays = 1;
            }
            decimal? totalPrice = booking.Room.FeePerNight * totalDays;

            booking.Status = "Da thanh toan";
            booking.IsPayment = true;
            _context.Update(booking);

            

            var invoice = new Invoice
            {
                BookingId = booking.BookingId,
                InvoiceDate = DateTime.Now,
                InvoiceAmount = totalPrice,
                IsPaid = true,
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Thanh toan thanh cong";
            return RedirectToAction("InvoiceDetails", new { id = invoice.InvoiceId});
        }

        public async Task<IActionResult> InvoiceDetails(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Booking)
                .ThenInclude(b => b.Room)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);
            if(invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        [ActionName("CheckOut")]
        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BookingId == id);


            if (booking == null)
            {
                return NotFound();
            }
            booking.Status = "CheckOut";
            _context.Update(booking);

            var room = await _context.Rooms.FindAsync(booking.RoomId);
            if (room != null)
            {
                room.IsAvailable = true;
                _context.Update(room);
                await _context.SaveChangesAsync();
            }

            TempData["MessageCheckOut"] = "Tra phong thanh cong";
            return RedirectToAction("Details", new { id = booking.BookingId});
        }



        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        [Authorize]
        public IActionResult Create(int? roomId)
        {
            var user = _userManager.GetUserId(User);
            
            if (roomId == null)
            {
                return NotFound();
            }
            return View("Create", new RoomListViewModel
            {
                Rooms = _context.Rooms
                .Include(r => r.Capacity)
                .Include(r => r.RoomType)
                .Include(r => r.Size)
                .Where(b => b.RoomId == roomId),

                Booking = new Booking
                {

                    RoomId = roomId.Value,
                    UserId = user,
                    BookingDate = DateTime.Now,
                    Status = "Confirmed",
                    IsPayment = false
                }

            });
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BookingId,UserId,RoomId,CheckInDate,CheckOutDate,BookingDate,Status")] Booking booking)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                if (room != null)
                {
                    room.IsAvailable = false;
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToAction("Details", new {id = booking.BookingId});
            }
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomNo", booking.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,UserId,RoomId,CheckInDate,CheckOutDate,BookingDate,Status")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomNo", booking.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
