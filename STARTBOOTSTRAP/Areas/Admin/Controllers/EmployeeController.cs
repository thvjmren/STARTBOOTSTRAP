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
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<GetEmployeeVM> employeeVMs = await _context.Employees.Select(e =>
                new GetEmployeeVM
                {
                    Name = e.Name,
                    Id = e.Id,
                    PositionName = e.Position.Name,
                    Image = e.Image,
                    Facebook = e.Facebook,
                    Linkedin = e.Linkedin,
                    X = e.X
                }
            ).ToListAsync();
            return View(employeeVMs);
        }

        public IActionResult Create()
        {
            CreateEmployeeVM employeeVM = new()
            {
                Positions = _context.Positions.ToList()
            };
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            employeeVM.Positions = await _context.Positions.ToListAsync();
            if (!ModelState.IsValid) return View();

            if (!employeeVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "file type is incorrect");
                return View(employeeVM);
            }

            if (!employeeVM.Photo.ValidateSize(FileSize.MB, 1))
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "file size should be less than 1MB");
                return View(employeeVM);
            }

            string fileName = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");

            Employee employee = new()
            {
                Name = employeeVM.Name,
                Image = fileName,
                Facebook = employeeVM.Facebook,
                X = employeeVM.X,
                Linkedin = employeeVM.Linkedin,
                PositionId = employeeVM.PositionId
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Employee? employee = await _context.Employees.Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (employee is null) return NotFound();

            List<Position> positions = await _context.Positions.ToListAsync();

            UpdateEmployeeVM employeeVM = new UpdateEmployeeVM
            {
                Name = employee.Name,
                Image = employee.Image,
                Facebook = employee.Facebook,
                Linkedin = employee.Linkedin,
                X = employee.X,
                PositionName = employee.Position.Name,
                Positions = positions
            };
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateEmployeeVM employeeVM)
        {
            employeeVM.Positions = await _context.Positions.ToListAsync();
            if (!ModelState.IsValid) return View();

            if (employeeVM.Photo is not null)
            {
                if (!employeeVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVM.Photo), "only image");
                    return View(employeeVM);
                }
                if (!employeeVM.Photo.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVM.Photo), "only less than 2MB");
                    return View(employeeVM);
                }
            }
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee is null)
            {
                return NotFound();
            }
            if (employeeVM.Photo is not null)
            {
                employee.Image.DeleteFile(_env.WebRootPath, "assets", "img");
                employee.Image = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            employee.Name = employeeVM.Name;
            employee.Facebook = employeeVM.Facebook;
            employee.X = employeeVM.X;
            employee.Linkedin = employeeVM.Linkedin;
            employee.PositionId = employeeVM.PositionId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Employee? employee = await _context.Employees.FirstOrDefaultAsync();

            if (employee is null) return NotFound();

            employee.Image.DeleteFile(_env.WebRootPath, "assets", "img");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}