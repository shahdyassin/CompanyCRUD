using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_2.Models;

namespace Task_2.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly CompanyDbContext _context;
        public DepartmentController(CompanyDbContext context)
        {
            _context = context;
        }

        public IActionResult TestAjax()
        {
            return View();
        }

        public IActionResult GetEmployeesAjax(int id)
        {
            var employees = _context.Employees
                            .Where(e => e.DepartmentId == id)
                            .ToList();
            if (employees.Count == 0)
            {
                return PartialView("_NoEmployees");
            }

            return PartialView("_EmployeesByDepartment", employees);
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments
                                           .Include(d => d.Employees)
                                           .FirstOrDefaultAsync(m => m.DepartmentId == id);

            if (department == null) return NotFound();

            return View(department);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,Name")] Department department)
        {
            if (id != department.DepartmentId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Departments.Any(e => e.DepartmentId == department.DepartmentId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
