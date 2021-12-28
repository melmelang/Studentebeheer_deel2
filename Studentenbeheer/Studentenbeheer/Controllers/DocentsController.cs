using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    [Authorize(Roles = "Beheerder")]
    public class DocentsController : AppController
    {
        private readonly UserManager<AppUser> _userManager;

        public DocentsController(UserManager<AppUser> userManager,
                                    AppDataContext context,
                                    IHttpContextAccessor httpContextAccessor,
                                    ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
            _userManager = userManager;
        }

        // GET: Docents
        public async Task<IActionResult> Index()
        {
            ViewData["genderId"] = new SelectList(_context.Gender.ToList(), "ID", "Name");
            return View(await _context.Docent.ToListAsync());
        }

        // GET: Docents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Docent = await _context.Docent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Docent == null)
            {
                return NotFound();
            }

            return View(Docent);
        }

        // GET: Docents/Create
        public IActionResult Create()
        {
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name");
            return View();
        }

        // POST: Docents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Voornaam,Achternaam,Geboortedatum,GeslachtId")] Docent docent)
        {
            if (ModelState.IsValid)
            {
                //user deel
                var user = Activator.CreateInstance<AppUser>();
                user.Voornaam = docent.Voornaam;
                user.Achternaam = docent.Achternaam;
                user.UserName = docent.Voornaam + "." + docent.Achternaam;
                user.Email = docent.Voornaam + "." + docent.Achternaam + "@ehb.be";
                user.EmailConfirmed = true;
                await _userManager.CreateAsync(user, docent.Voornaam + "." + docent.Achternaam + "EHB1");

                //docent deel
                docent.UserId = user.Id;
                _context.Add(docent);

                await _context.SaveChangesAsync();

                await _userManager.AddToRoleAsync(user, "Docent");
                return RedirectToAction(nameof(Index));
            }
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", docent.GeslachtId);
            return View(docent);
        }

        // GET: Docents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Docent = await _context.Docent.FindAsync(id);
            if (Docent == null)
            {
                return NotFound();
            }
            return View(Docent);
        }

        // POST: Docents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Voornaam,Achternaam,Geboortedatum,GeslachtId")] Docent docent)
        {
            if (id != docent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocentExists(docent.Id))
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
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", docent.GeslachtId);
            return View(docent);
        }

        // GET: Docents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Docent = await _context.Docent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Docent == null)
            {
                return NotFound();
            }

            return View(Docent);
        }

        // POST: Docents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Docent = await _context.Docent.FindAsync(id);
            //_context.Docent.Remove(Docent);
            Docent.Deleted = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocentExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
