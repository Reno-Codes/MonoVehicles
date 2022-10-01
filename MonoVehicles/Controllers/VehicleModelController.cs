using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonoVehicles.Models;

namespace MonoVehicles.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly AnotherAppDBContext _context;

        public VehicleModelController(AnotherAppDBContext context)
        {
            _context = context;
        }

        // GET: VehicleModel
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

            var vehicleModels = from v in _context.VehicleModels
                                select v;
            // Searching
            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleModels = vehicleModels.Where(s => s.Name.Contains(searchString)
                                       || s.Abrv.Contains(searchString)
                                       || s.Id.ToString().Contains(searchString));
            }
            // Sorting
            vehicleModels = sortOrder switch
            {
                "name_desc" => vehicleModels.OrderByDescending(v => v.Name),
                "Abrv" => vehicleModels.OrderBy(s => s.Abrv),
                "abrv_desc" => vehicleModels.OrderByDescending(v => v.Abrv),
                "Id" => vehicleModels.OrderBy(s => s.Id),
                "id_desc" => vehicleModels.OrderByDescending(s => s.Id),
                _ => vehicleModels.OrderBy(s => s.Name),
            };
            //return View(await _context.VehicleModels.ToListAsync());
            int pageSize = 5;
            return View(await PaginatedList<VehicleModel>.CreateAsync(vehicleModels.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: VehicleModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VehicleModels == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            return View(vehicleModel);
        }

        // GET: VehicleModel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MakeId,Name,Abrv")] VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleModel);
        }

        // GET: VehicleModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VehicleModels == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModels.FindAsync(id);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            return View(vehicleModel);
        }

        // POST: VehicleModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MakeId,Name,Abrv")] VehicleModel vehicleModel)
        {
            if (id != vehicleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleModelExists(vehicleModel.Id))
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
            return View(vehicleModel);
        }

        // GET: VehicleModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VehicleModels == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            return View(vehicleModel);
        }

        // POST: VehicleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VehicleModels == null)
            {
                return Problem("Entity set 'AnotherAppDBContext.VehicleModels'  is null.");
            }
            var vehicleModel = await _context.VehicleModels.FindAsync(id);
            if (vehicleModel != null)
            {
                _context.VehicleModels.Remove(vehicleModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleModelExists(int id)
        {
            return _context.VehicleModels.Any(e => e.Id == id);
        }
    }
}
