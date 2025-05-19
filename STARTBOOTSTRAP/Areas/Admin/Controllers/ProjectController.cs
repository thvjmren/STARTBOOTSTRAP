using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Utilities.Extensions;
using STARTBOOTSTRAP.DAL;
using STARTBOOTSTRAP.Models;
using STARTBOOTSTRAP.Utilities.Enums;
using STARTBOOTSTRAP.ViewModels;

namespace STARTBOOTSTRAP.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProjectController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<GetProjectVM> projectVMs = await _context.Projects.Select(p =>
            new GetProjectVM
            {
                Name = p.Name,
                CategoryName = p.Category.Name,
                Id = p.Id,
                Image = p.Image
            }).ToListAsync();
            return View(projectVMs);
        }

        public async Task<IActionResult> Create()
        {
            CreateProjectVM projectVM = new()
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(projectVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectVM projectVM)
        {
            projectVM.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid) return View(projectVM);

            if (!projectVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateProjectVM.Photo), "file type is incorrect");
                return View(projectVM);
            }

            if (!projectVM.Photo.ValidateSize(FileSize.MB, 1))
            {
                ModelState.AddModelError(nameof(CreateProjectVM.Photo), "file size should be less than 1MB");
                return View(projectVM);
            }

            string fileName = await projectVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "portfolio");

            Project project = new()
            {
                Name = projectVM.Name,
                CategoryId = projectVM.CategoryId,
                Image = fileName
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
