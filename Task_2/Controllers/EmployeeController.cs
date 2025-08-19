using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task_2.Models;

namespace Task_2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly CompanyDbContext _context;
        public EmployeeController(CompanyDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                                          .Include(e => e.Department)
                                          .ToListAsync();
            return View(employees);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                                         .Include(e => e.Department)
                                         .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null) return NotFound();

            return View(employee);
        }
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,DepartmentId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,DepartmentId")] Employee employee)
        {
            if (id != employee.EmployeeId) return NotFound();

            if (ModelState.IsValid)
            {
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null) return NotFound();

                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.DepartmentId = employee.DepartmentId;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", employee.DepartmentId);
            return View(employee);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
