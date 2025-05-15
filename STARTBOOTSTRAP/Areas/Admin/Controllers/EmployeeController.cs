using Microsoft.AspNetCore.Mvc;
using STARTBOOTSTRAP.DAL;
using STARTBOOTSTRAP.ViewModels;

namespace STARTBOOTSTRAP.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetEmployeeVM> employeeVMs = await _context.Employees.Select(e=>
            )
            return View();
        }
    }
}
