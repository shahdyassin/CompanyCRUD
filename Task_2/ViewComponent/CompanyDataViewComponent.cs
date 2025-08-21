using Microsoft.AspNetCore.Mvc;
using Task_2.Models;

namespace Task_2
{
    public class CompanyDataViewComponent : ViewComponent
    {
        private readonly CompanyDbContext _context;
        public CompanyDataViewComponent(CompanyDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var totalEmployees = _context.Employees.Count();
            var totalDepartments = _context.Departments.Count();

            
            var avgSalary = 0;

            var model = new
            {
                TotalEmployees = totalEmployees,
                TotalDepartments = totalDepartments,
                AverageSalary = avgSalary
            };

            return View(model);
        }
    }
}
