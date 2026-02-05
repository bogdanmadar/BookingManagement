using BookingOct.Models;
using BookingOct.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookingOct.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BookingsController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var query = _context.Bookings.Include(b => b.Room).AsQueryable();
            if (!User.IsInRole("Admin"))
                query = query.Where(b => b.UserId == User.Identity.Name);
            return View(await query.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            bool conflict = await _context.Bookings.AnyAsync(b =>
                b.RoomId == booking.RoomId &&
                booking.StartDate < b.EndDate &&
                booking.EndDate > b.StartDate);

            if (conflict)
            {
                ModelState.AddModelError("", "Camera este deja ocupată în acest interval!");
                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
                return View(booking);
            }

            booking.UserId = User.Identity.Name;
            _context.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }


        [HttpGet]
        public async Task<IActionResult> GetCalendarData(int roomId)
        {
            var events = await _context.Bookings.Where(b => b.RoomId == roomId)
                .Select(b => new { title = "Ocupat", start = b.StartDate, end = b.EndDate }).ToListAsync();
            return Json(events);
        }
    }
}