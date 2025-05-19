
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
                Name = positionVM.Name
            };

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);

            if (position is null) return NotFound();

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);

            if (position is null) return NotFound();

            UpdatePositionVM positionVM = new()
            {
                Name = position.Name
            };
            return View(positionVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdatePositionVM positionVM)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool result = await _context.Positions.AnyAsync(p => p.Id != id && p.Name == positionVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(Position.Name), $"position:{positionVM.Name} is already exists");
                return View();
            }

            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (position.Name == positionVM.Name)
            {
                return RedirectToAction(nameof(Index));
            }

            position.Name = positionVM.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
