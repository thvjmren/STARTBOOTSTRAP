
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STARTBOOTSTRAP.DAL;
using STARTBOOTSTRAP.Models;
using STARTBOOTSTRAP.ViewModels;

namespace STARTBOOTSTRAP.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<GetPositionVM> positionVMs = await _context.Positions.Select(p =>

                new GetPositionVM
                {
                    Name = p.Name,
                    Id = p.Id
                }
            ).ToListAsync();
            return View(positionVMs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionVM positionVM)
        {
            if (!ModelState.IsValid) return View();

            bool result = await _context.Positions.AnyAsync(p => p.Name == positionVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(positionVM.Name), $"the position: {positionVM.Name} is already exist");
                return View();
            }

            Position position = new()
            {
                Name = positionVM.Name,
            };

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Position? position = await _context.Positions.FirstOrDefaultAsync(p=>p.Id == id);

            if (position is null) return NotFound();

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
