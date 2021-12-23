﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Data;
using Studentenbeheer.Models;
using Microsoft.AspNetCore.Authorization;
using Studentenbeheer.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Studentenbeheer.Controllers
{
    [Authorize (Roles = "Beheerder")]
    public class StudentsController : AppController
    {
        private readonly UserManager<AppUser> _userManager;

        public StudentsController(UserManager<AppUser> userManager, 
                                    AppDataContext context, 
                                    IHttpContextAccessor httpContextAccessor, 
                                    ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
            _userManager = userManager;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchFieldName, char searchGender, string orderBy)
        {
            var students = from s in _context.Student where s.Deleted > DateTime.Now orderby s.Voornaam select s;
            var user = _context.Users.FirstOrDefault(e => e.UserName == User.Identity.Name);
            var roles = _context.UserRoles.Where(r => r.UserId == user.Id);
            var idertityroles = User.IsInRole("Guest");

            if (searchGender != 0)
                students = from s in _context.Student
                           where s.GeslachtId == searchGender
                           orderby s.Voornaam
                           select s;

            if (!string.IsNullOrEmpty(searchFieldName))
                students = from s in students
                           where s.Achternaam.Contains(searchFieldName) || s.Voornaam.Contains(searchFieldName)
                           orderby s.Achternaam, s.Voornaam
                           select s;

            ViewData["VoornaamField"] = orderBy == "Voornaam" ? "Voornaam_Desc" : "Voornaam";
            ViewData["AchternaamField"] = orderBy == "Achternaam" ? "Achetrnaam_Desc" : "Achternaam";
            ViewData["GeboortedatumField"] = string.IsNullOrEmpty(orderBy) ? "Geboortedatum_Desc" : "";
            ViewData["genderId"] = new SelectList(_context.Gender.ToList(), "ID", "Name");

            switch (orderBy)
            {
                case "Voornaam":
                    students = students.OrderBy(s => s.Voornaam);
                    break;
                case "Voornaam_Desc":
                    students = students.OrderByDescending(s => s.Voornaam);
                    break;
                case "Achternaam":
                    students = students.OrderBy(s => s.Achternaam);
                    break;
                case "Achernaam_Desc":
                    students = students.OrderByDescending(s => s.Achternaam);
                    break;
                case "Geboortedatum_Desc":
                    students = students.OrderByDescending(s => s.Geboortedatum);
                    break;
                default:
                    students = students.OrderBy(s => s.Geboortedatum);
                    break;
            }

            IQueryable<Gender> genderToSelect = from g in _context.Gender orderby g.Name select g;

            StudentIndexViewModel studentIndexViewModel = new StudentIndexViewModel()
            {
                NaamFilter = searchFieldName,
                GeslachtIdFilter = searchGender,
                FilteredStudent = await students.Include(g => g.Geslacht).ToListAsync(),
                GenderToSelect = new SelectList(await genderToSelect.ToListAsync(), "Id", "Name", searchGender)
            };
            return View(studentIndexViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Geslacht)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            string currentUserId = User.Identity.Name;
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Voornaam,Achternaam,Geboortedatum,GeslachtId")] Student student)
        {
            if (ModelState.IsValid)
            {
                //user deel
                var user = Activator.CreateInstance<AppUser>();
                user.Voornaam = student.Voornaam;
                user.Achternaam = student.Achternaam;
                user.UserName = student.Voornaam + "." + student.Achternaam;
                user.Email = student.Voornaam + "." + student.Achternaam + "@ehb.be";
                user.EmailConfirmed = true;
                await _userManager.CreateAsync(user, student.Voornaam + "." + student.Achternaam + "EHB1");

                //student deel
                student.UserId = user.Id;
                _context.Add(student);

                await _context.SaveChangesAsync();

                await _userManager.AddToRoleAsync(user, "Student");
                return RedirectToAction(nameof(Index));
            }
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", student.GeslachtId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", student.GeslachtId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Voornaam,Achternaam,Geboortedatum,GeslachtId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", student.GeslachtId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Geslacht)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            //_context.Student.Remove(student);
            student.Deleted = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
