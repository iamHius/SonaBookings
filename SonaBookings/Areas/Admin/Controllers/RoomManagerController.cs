using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SonaBookings.Areas.Admin.Interfaces;
using SonaBookings.Areas.Identity.Data;
using SonaBookings.Models;

namespace SonaBookings.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomManagerController : Controller
    {
        private readonly IRoomFactory _roomFactory;

        public RoomManagerController(IRoomFactory roomFactory)
        {
            _roomFactory = roomFactory;
        }

        // GET: Admin/RoomManager
        public async Task<IActionResult> Index()
        {
            var rooms = await _roomFactory.GetAllRoomsAsync();
            return View(rooms);
        }

        // GET: Admin/RoomManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomFactory.GetRoomByIdAsync(id.Value);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Admin/RoomManager/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CapacityId"] = new SelectList(await _roomFactory.GetAllCapacitiesAsync(), "CapacityId", "CapacityName");
            ViewData["RoomTypeId"] = new SelectList(await _roomFactory.GetAllRoomTypesAsync(), "RoomTypeId", "RoomTypeName");
            ViewData["SizeId"] = new SelectList(await _roomFactory.GetAllSizesAsync(), "SizeId", "SizeName");
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
                await _roomFactory.AddRoomAsync(room);

                return RedirectToAction(nameof(Index));
            }
            ViewData["CapacityId"] = new SelectList(await _roomFactory.GetAllCapacitiesAsync(), "CapacityId", "CapacityName", room.CapacityId);
            ViewData["RoomTypeId"] = new SelectList(await _roomFactory.GetAllRoomTypesAsync(), "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            ViewData["SizeId"] = new SelectList(await _roomFactory.GetAllSizesAsync(), "SizeId", "SizeName", room.SizeId);
            return View(room);
        }

        // GET: Admin/RoomManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomFactory.GetRoomByIdAsync(id.Value);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["CapacityId"] = new SelectList(await _roomFactory.GetAllCapacitiesAsync(), "CapacityId", "CapacityName", room.CapacityId);
            ViewData["RoomTypeId"] = new SelectList(await _roomFactory.GetAllRoomTypesAsync(), "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            ViewData["SizeId"] = new SelectList(await _roomFactory.GetAllSizesAsync(), "SizeId", "SizeName", room.SizeId);
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
                    await _roomFactory.UpdateRoomAsync(room);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CapacityId"] = new SelectList(await _roomFactory.GetAllCapacitiesAsync(), "CapacityId", "CapacityName", room.CapacityId);
            ViewData["RoomTypeId"] = new SelectList(await _roomFactory.GetAllRoomTypesAsync(), "RoomTypeId", "RoomTypeName", room.RoomTypeId);
            ViewData["SizeId"] = new SelectList(await _roomFactory.GetAllSizesAsync(), "SizeId", "SizeName", room.SizeId);
            return View(room);
        }

        // GET: Admin/RoomManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomFactory.GetRoomByIdAsync(id.Value);
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
            await _roomFactory.DeleteRoomAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
