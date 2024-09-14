using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SonaBookings.Areas.Identity.Data;
using SonaBookings.Models;

namespace SonaBookings.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/RoomManager
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rooms.Include(r => r.Capacity).Include(r => r.RoomType).Include(r => r.Size);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/RoomManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Capacity)
                .Include(r => r.RoomType)
                .Include(r => r.Size)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Admin/RoomManager/Create
        public IActionResult Create()
        {
            ViewData["CapacityId"] = new SelectList(_context.Capacitys, "CapacityId", "CapacityName");
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName");
            return View();
        }

        // POST: Admin/RoomManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,RoomNo,FeePerNight,RoomPhoto,IsAvailable,RoomDescription,RoomTypeId,SizeId,CapacityId")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CapacityId"] = new SelectList(_context.Capacitys, "CapacityId", "CapacityName", room.CapacityId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", room.SizeId);
            return View(room);
        }

        // GET: Admin/RoomManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["CapacityId"] = new SelectList(_context.Capacitys, "CapacityId", "CapacityName", room.CapacityId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", room.SizeId);
            return View(room);
        }

        // POST: Admin/RoomManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomNo,FeePerNight,RoomPhoto,IsAvailable,RoomDescription,RoomTypeId,SizeId,CapacityId")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
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
            ViewData["CapacityId"] = new SelectList(_context.Capacitys, "CapacityId", "CapacityName", room.CapacityId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", room.SizeId);
            return View(room);
        }

        // GET: Admin/RoomManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Capacity)
                .Include(r => r.RoomType)
                .Include(r => r.Size)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Admin/RoomManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomId == id);
        }
    }
}
