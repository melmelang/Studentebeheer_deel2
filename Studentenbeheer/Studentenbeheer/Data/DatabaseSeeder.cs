using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Data
{
    public class DatabaseSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider, UserManager<AppUser> userManager)
        {
            using (var context = new AppDataContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDataContext>>()))
            {
                AppUser user = null;
                AppUser user2 = null;
                AppUser user3 = null;
                AppUser user4 = null;
                AppUser user5 = null;
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    user = new AppUser
                    {
                        Voornaam = "Melvin",
                        Achternaam = "Angeli",
                        UserName = "Melvin.Angeli",
                        Email = "Angeli.melvin@hotmail.com",
                        EmailConfirmed = true,
                    };
                    user2 = new AppUser
                    {
                        Voornaam = "Antoine",
                        Achternaam = "Couck",
                        UserName = "Antoine.Couck",
                        Email = "Angeli.melvin@hotmail.com",
                        EmailConfirmed = true,
                    };
                    user3 = new AppUser
                    {
                        Voornaam = "Ine",
                        Achternaam = "DeBast",
                        UserName = "Ine.DeBast",
                        Email = "Angeli.melvin@hotmail.com",
                        EmailConfirmed = true,
                    };
                    user4 = new AppUser
                    {
                        Voornaam = "Tilly",
                        Achternaam = "VanLaethem",
                        UserName = "Tilly.VanLaethem",
                        Email = "Angeli.melvin@hotmail.com",
                        EmailConfirmed = true,
                    };
                    user5 = new AppUser
                    {
                        Voornaam = "Brian",
                        Achternaam = "Angeli",
                        UserName = "Brian.Angeli",
                        Email = "Angeli.melvin@hotmail.com",
                        EmailConfirmed = true,
                    };
                    userManager.CreateAsync(user, "Student+1");
                    userManager.CreateAsync(user2, "Student+1");
                    userManager.CreateAsync(user3, "Student+1");
                    userManager.CreateAsync(user4, "Student+1");
                    userManager.CreateAsync(user5, "Student+1");

                    context.Roles.AddRange(
                        new IdentityRole { Id = "Beheerder", Name = "Beheerder", NormalizedName = "beheerder" },
                        new IdentityRole { Id = "Docent", Name = "Docent", NormalizedName = "docent" },
                        new IdentityRole { Id = "Student", Name = "Student", NormalizedName = "student" }
                        );
                    context.SaveChanges();
                }

                if (!context.Gender.Any())
                {
                    context.Gender.AddRange(

                        new Gender
                        {
                            ID = 'M',
                            Name = "Man"
                        },

                        new Gender
                        {
                            ID = 'V',
                            Name = "Vrouw"
                        },

                        new Gender
                        {
                            ID = '-',
                            Name = "None"
                        }

                    );
                    context.SaveChanges();
                }

                if (!context.Student.Any())
                {
                    context.Student.AddRange(

                        new Student
                        {
                            Voornaam = user2.Voornaam,
                            Achternaam = user2.Achternaam,
                            Geboortedatum = DateTime.Now,
                            GeslachtId = 'V',
                            UserId = user2.Id,
                            Deleted = DateTime.MaxValue
                        },

                        new Student
                        {
                            Voornaam = user3.Voornaam,
                            Achternaam = user3.Achternaam,
                            Geboortedatum = DateTime.Now,
                            GeslachtId = 'M',
                            UserId = user3.Id,
                            Deleted = DateTime.MaxValue
                        }

                    );
                    context.SaveChanges();
                }

                if (!context.Docent.Any())
                {
                    context.Docent.AddRange(

                        new Docent
                        {
                            Voornaam = user4.Voornaam,
                            Achternaam = user4.Achternaam,
                            Geboortedatum = DateTime.Now,
                            GeslachtId = 'V',
                            UserId = user4.Id,
                            Deleted = DateTime.MaxValue
                        },

                        new Docent
                        {
                            Voornaam = user5.Voornaam,
                            Achternaam = user5.Achternaam,
                            Geboortedatum = DateTime.Now,
                            GeslachtId = 'M',
                            UserId = user5.Id,
                            Deleted = DateTime.MaxValue
                        }

                    );
                    context.SaveChanges();
                }

                if (!context.Module.Any())
                {
                    context.Module.AddRange(

                        new Module
                        {
                            Naam = "Programmeren",
                            Omschrijving = "Dingens programmeren",
                            Deleted = DateTime.MaxValue
                        },

                        new Module
                        {
                            Naam = "Disign",
                            Omschrijving = "Dingens tekenen",
                            Deleted = DateTime.MaxValue
                        },

                        new Module
                        {
                            Naam = "-",
                            Omschrijving = "-",
                            Deleted = DateTime.Now
                        }

                    );
                    context.SaveChanges();
                }

                if (!context.Inschrijvingen.Any())
                {
                    context.Inschrijvingen.AddRange(
                        new Inschrijvingen
                        {
                            StudentId = 1,
                            ModuleId = 1,
                            Inschrijvingsdatum = new DateTime(666, 1, 1),
                        },
                        new Inschrijvingen
                        {
                            StudentId = 1,
                            ModuleId = 2,
                            Inschrijvingsdatum = DateTime.MinValue,
                        },
                        new Inschrijvingen
                        {
                            StudentId = 2,
                            ModuleId = 1,
                            Inschrijvingsdatum = DateTime.MinValue,
                        }
                        );
                    context.SaveChanges();
                }

                if (!context.DocentModule.Any())
                {
                    context.DocentModule.AddRange(
                        new DocentModule
                        {
                            DocentId = 1,
                            ModuleId = 1
                        },
                        new DocentModule
                        {
                            DocentId = 2,
                            ModuleId = 2
                        },
                        new DocentModule
                        {
                            DocentId = 2,
                            ModuleId = 2
                        }
                        );
                    context.SaveChanges();
                }

                if (user != null)
                {
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string> { RoleId = "Beheerder", UserId = user.Id },
                        new IdentityUserRole<string> { RoleId = "Docent", UserId = user4.Id },
                        new IdentityUserRole<string> { RoleId = "Docent", UserId = user5.Id },
                        new IdentityUserRole<string> { RoleId = "Student", UserId = user2.Id },
                        new IdentityUserRole<string> { RoleId = "Student", UserId = user3.Id }
                        );
                    context.SaveChanges();
                }
            }
        }
    }
}
