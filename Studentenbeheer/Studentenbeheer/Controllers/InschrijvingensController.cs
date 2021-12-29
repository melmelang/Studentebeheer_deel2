using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    [Authorize(Roles = "Student,Beheerder,Docent")]
    public class InschrijvingensController : AppController
    {

        public InschrijvingensController(AppDataContext context, IHttpContextAccessor httpContextAccessor, ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        // GET: Inschrijvingens
        public async Task<IActionResult> Index()
        {
            var studentenbeheerContext = _context.Inschrijvingen.Include(i => i.Module).Include(i => i.Student);
            if (User.IsInRole("Beheerder"))
            {
                return View(await studentenbeheerContext.ToListAsync());
            }
            if (User.IsInRole("Docent"))
            {
                var moduleId = _context.DocentModule.Include(dm => dm.Module).Include(dm => dm.Docent)
                    .Where(dm => dm.Docent.UserId == _user.Id)
                    .Select(dm => dm.Module.Id).ToList();
                var studentenbeheerContext2 = _context.Inschrijvingen.Include(i => i.Module).Include(i => i.Student)
                    .Where(i => moduleId.Contains(i.Module.Id));
                return View(await studentenbeheerContext2.ToListAsync());
            }
            if (User.IsInRole("Student"))
            {
                var studentenbeheerContext2 = _context.Inschrijvingen.Include(i => i.Module).Include(i => i.Student)
                    .Where(i => i.Student.UserId == _user.Id);
                return View(await studentenbeheerContext2.ToListAsync());

            }
            return View(await studentenbeheerContext.ToListAsync());
        }

        // GET: Inschrijvingens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen
                .Include(i => i.Module)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }

            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]
        // GET: Inschrijvingens/Create
        public IActionResult Create(int? id, int? wich)
        {
            var module = _context.Module.Where(m => m.Deleted > DateTime.Now);
            var student = _context.Student.Where(s => s.Deleted > DateTime.Now);
            ViewData["ModuleId"] = new SelectList(module, "Id", "Naam");
            ViewData["StudentId"] = new SelectList(student, "Id", "Achternaam");

            if (wich == 1)
            {
                module = _context.Module.Where(m => m.Id == id);
                ViewData["ModuleId"] = new SelectList(module, "Id", "Naam");
            }
            if (wich == 2)
            {
                student = _context.Student.Where(s => s.Id == id);
                ViewData["StudentId"] = new SelectList(student, "Id", "Achternaam");
            }
            return View();
        }

        [Authorize(Roles = "Beheerder")]
        // POST: Inschrijvingens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, int? wich, [Bind("Id,StudentId,ModuleId,Inschrijvingsdatum")] Inschrijvingen inschrijvingen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inschrijvingen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Naam", inschrijvingen.ModuleId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Achternaam", inschrijvingen.StudentId);
            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]
        // GET: Inschrijvingens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen.FindAsync(id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Naam", inschrijvingen.ModuleId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Achternaam", inschrijvingen.StudentId);
            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]
        // POST: Inschrijvingens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,ModuleId,Inschrijvingsdatum,AfgelegdOp,Resultaat")] Inschrijvingen inschrijvingen)
        {
            if (id != inschrijvingen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inschrijvingen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InschrijvingenExists(inschrijvingen.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Naam", inschrijvingen.ModuleId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Achternaam", inschrijvingen.StudentId);
            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]
        // GET: Inschrijvingens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen
                .Include(i => i.Module)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }

            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]
        // POST: Inschrijvingens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inschrijvingen = await _context.Inschrijvingen.FindAsync(id);
            _context.Inschrijvingen.Remove(inschrijvingen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InschrijvingenExists(int id)
        {
            return _context.Inschrijvingen.Any(e => e.Id == id);
        }
    }
}
