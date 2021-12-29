using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    [Authorize(Roles = "Beheerder")]
    public class AccountManagerController : AppController
    {
        public AccountManagerController(AppDataContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AppController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        public IActionResult Index(string userName, string name, string email, int? pageNumber)
        {
            if (userName == null) userName = "";
            if (name == null) name = "";
            if (email == null) email = "";
            List<AppUser> users =
                _context.Users.ToList()
                .Where(u => (userName == "" || u.UserName.Contains(userName))
                         && (name == "" || (u.Voornaam.Contains(name) || u.Achternaam.Contains(name)))
                         && (email == "" || u.Email.Contains(email)))
                .OrderBy(u => u.Voornaam + " " + u.Voornaam)
                .ToList();
            List<AppUserViewModel> userViewModels = new List<AppUserViewModel>();
            foreach (var user in users)
            {
                userViewModels.Add(new AppUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Voornaam = user.Voornaam,
                    Achternaam = user.Achternaam,
                    Lockout = user.LockoutEnd != null,
                    PhoneNumber = user.PhoneNumber,
                    Student = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Student").Count() > 0,
                    Docent = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Docent").Count() > 0,
                    Beheerder = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Beheerder").Count() > 0
                });

            }
            ViewData["userName"] = userName;
            ViewData["name"] = name;
            ViewData["email"] = email;
            if (pageNumber == null) pageNumber = 1;
            PageList<AppUserViewModel> model = new PageList<AppUserViewModel>(userViewModels, userViewModels.Count, 1, 10);
            return View(model);
        }

        public async Task<ActionResult> Locking(string id)
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user.LockoutEnd != null)
                user.LockoutEnd = null;
            else
                user.LockoutEnd = new DateTimeOffset(DateTime.Now + new TimeSpan(7, 0, 0, 0));
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Roles(string id)
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            AppUserViewModel model = new AppUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Voornaam = user.Voornaam,
                Achternaam = user.Achternaam,
                Lockout = user.LockoutEnd != null,
                PhoneNumber = user.PhoneNumber,
                Student = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Student").Count() > 0,
                Docent = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Docent").Count() > 0,
                Beheerder = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Beheerder").Count() > 0
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Roles([Bind("Id, UserName, Voornaam, AchterNaam, Student, Docent, Beheerder")] AppUserViewModel model)
        {
            List<IdentityUserRole<string>> roles = _context.UserRoles.Where(ur => ur.UserId == model.Id).ToList();
            foreach (IdentityUserRole<string> role in roles)
            {
                _context.Remove(role);
            }
            if (model.Student) _context.Add(new IdentityUserRole<string> { RoleId = "Student", UserId = model.Id });
            if (model.Docent) _context.Add(new IdentityUserRole<string> { RoleId = "Docent", UserId = model.Id });
            if (model.Beheerder) _context.Add(new IdentityUserRole<string> { RoleId = "Beheerder", UserId = model.Id });
            await _context.SaveChangesAsync();
            ; return RedirectToAction("Index");
        }
    }

}
