using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STARTBOOTSTRAP.DAL;
using STARTBOOTSTRAP.Models;
using STARTBOOTSTRAP.ViewModels;

namespace STARTBOOTSTRAP.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            HomeVM vm = new HomeVM
            {
                Projects = await _context.Projects.Include(p => p.Category).ToListAsync(),
                Employees = await _context.Employees.Include(e => e.Position).ToListAsync(),
            };

            return View(vm);
        }
    }
}
