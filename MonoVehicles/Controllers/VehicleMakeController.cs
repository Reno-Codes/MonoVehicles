using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonoVehicles.Models;

namespace MonoVehicles.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly AnotherAppDBContext _context;

        public VehicleMakeController(AnotherAppDBContext context)
        {
            _context = context;
        }

        // GET: VehicleMake
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            // Sorting and Searching
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AbrvSortParm"] = sortOrder == "Abrv" ? "abrv_desc" : "Abrv";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var vehicleMakes = from v in _context.VehicleMakes
                               select v;
            // Searching
            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleMakes = vehicleMakes.Where(s => s.Name.Contains(searchString)
                                       || s.Abrv.Contains(searchString)
                                       || s.Id.ToString().Contains(searchString));
            }
            // Sorting
            vehicleMakes = sortOrder switch
            {
                "name_desc" => vehicleMakes.OrderByDescending(v => v.Name),
                "Abrv" => vehicleMakes.OrderBy(s => s.Abrv),
                "abrv_desc" => vehicleMakes.OrderByDescending(v => v.Abrv),
                "Id" => vehicleMakes.OrderBy(v => v.Id),
                "id_desc" => vehicleMakes.OrderByDescending(v => v.Id),
                _ => vehicleMakes.OrderBy(s => s.Name),
            };
            //return View(await _context.vehicleMakes.ToListAsync());
            int pageSize = 5;
            return View(await PaginatedList<VehicleMake>.CreateAsync(vehicleMakes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: VehicleMake/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VehicleMakes == null)
            {
                return NotFound();
            }

            var vehicleMake = await _context.VehicleMakes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleMake == null)
            {
                return NotFound();
            }

            return View(vehicleMake);
        }

        // GET: VehicleMake/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleMake/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Abrv")] VehicleMake vehicleMake)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicleMake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleMake);
        }

        // GET: VehicleMake/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VehicleMakes == null)
            {
                return NotFound();
            }

            var vehicleMake = await _context.VehicleMakes.FindAsync(id);
            if (vehicleMake == null)
            {
                return NotFound();
            }
            return View(vehicleMake);
        }

        // POST: VehicleMake/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Abrv")] VehicleMake vehicleMake)
        {
            if (id != vehicleMake.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleMake);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleMakeExists(vehicleMake.Id))
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
            return View(vehicleMake);
        }

        // GET: VehicleMake/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VehicleMakes == null)
            {
                return NotFound();
            }

            var vehicleMake = await _context.VehicleMakes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleMake == null)
            {
                return NotFound();
            }

            return View(vehicleMake);
        }

        // POST: VehicleMake/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VehicleMakes == null)
            {
                return Problem("Entity set 'AnotherAppDBContext.VehicleMakes'  is null.");
            }
            var vehicleMake = await _context.VehicleMakes.FindAsync(id);
            if (vehicleMake != null)
            {
                _context.VehicleMakes.Remove(vehicleMake);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleMakeExists(int id)
        {
            return _context.VehicleMakes.Any(e => e.Id == id);
        }
    }
}
