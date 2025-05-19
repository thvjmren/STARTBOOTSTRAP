using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STARTBOOTSTRAP.DAL;
using STARTBOOTSTRAP.Models;
using STARTBOOTSTRAP.ViewModels;

namespace STARTBOOTSTRAP.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetCategoryVM> categoryVMs = await _context.Categories.Select(c =>
            new GetCategoryVM
            {
                Name = c.Name,
                Id = c.Id
            }).ToListAsync();
            return View(categoryVMs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View();

            bool result = await _context.Categories.AnyAsync(c => c.Name == categoryVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(categoryVM.Name), $"the position: {categoryVM.Name} is already exist");
                return View();
            }

            Category category = new()
            {
                Name = categoryVM.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
