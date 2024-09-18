using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SonaBookings.Areas.Identity.Data;
using SonaBookings.Models;
using SonaBookings.Models.ViewModels;


namespace SonaBookings.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public int PageSize = 6;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        [AllowAnonymous]
        public async Task<IActionResult> Index(int roompage = 1)
        {
            /*var applicationDbContext = _context.Rooms.Include(r => r.Capacity).Include(r => r.RoomType).Include(r => r.Size);
            return View(await applicationDbContext.ToListAsync());*/
            return View(new RoomListViewModel
            {
                Rooms = _context.Rooms.Skip((roompage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = roompage,
                    TotalItem = _context.Rooms.Count()
                }
            });
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string keywords, int roompage = 1)
        {
            return View(new RoomListViewModel
            {
                Rooms = _context.Rooms.Where(r => r.RoomNo.Contains(keywords))
                .Skip((roompage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = roompage,
                    TotalItem = _context.Rooms.Count()
                }
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CheckRoomByType(int? roomTypeId, int roompage = 1)
        {
            if (roomTypeId == null)
            {
                return View("NotFound");
            }    

            return View("Index", new RoomListViewModel
            {
                Rooms = _context.Rooms
                .Where(r => r.RoomTypeId == roomTypeId)
                .Include(r => r.RoomType)
                .Skip((roompage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = roompage,
                    TotalItem = _context.Rooms.Count()
                }
            });
        }


        // GET: Rooms/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var room = await _context.Rooms
                .Include(r => r.Capacity)
                .Include(r => r.RoomType)
                .Include(r => r.Size)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return View("NotFound");
            }

            return View(room);
        }

        // GET: Rooms/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CapacityId"] = new SelectList(_context.Capacitys, "CapacityId", "CapacityName");
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        // GET: Rooms/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return View("NotFound");
            }
            ViewData["CapacityId"] = new SelectList(_context.Capacitys, "CapacityId", "CapacityName", room.CapacityId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", room.SizeId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomNo,FeePerNight,RoomPhoto,IsAvailable,RoomDescription,RoomTypeId,SizeId,CapacityId")] Room room)
        {
            if (id != room.RoomId)
            {
                return View("NotFound");
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
                        return View("NotFound");
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

        // GET: Rooms/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var room = await _context.Rooms
                .Include(r => r.Capacity)
                .Include(r => r.RoomType)
                .Include(r => r.Size)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return View("NotFound");
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
