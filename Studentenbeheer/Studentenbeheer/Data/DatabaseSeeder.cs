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
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    user = new AppUser
                    {
                        Voornaam = "Melvin",
                        Achternaam = "Angeli",
                        UserName = "Melvin_Angeli",
                        Email = "Angeli.melvin@hotmail.com",
                        EmailConfirmed = true,
                    };
                    userManager.CreateAsync(user, "V@c@nc3s");

                    context.Roles.AddRange(
                        new IdentityRole { Id = "Beheerder", Name = "Beheerder", NormalizedName = "beheerder" },
                        new IdentityRole { Id = "Docent", Name = "Docent", NormalizedName = "docent" },
                        new IdentityRole { Id = "Student", Name = "Student", NormalizedName = "student"}
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
                            Voornaam = "Ine",
                            Achternaam = "DeBast",
                            Geboortedatum = DateTime.Now,
                            GeslachtId = 'V',
                            UserId = user.Id,
                            Deleted = DateTime.MaxValue
                        },

                        new Student
                        {
                            Voornaam = "Antoine",
                            Achternaam = "Couck",
                            Geboortedatum = DateTime.Now,
                            GeslachtId = 'M',
                            UserId = user.Id,
                            Deleted = DateTime.MaxValue
                        },

                        new Student
                        {
                            Voornaam = "Melvin",
                            Achternaam = "Angeli",
                            Geboortedatum = DateTime.Now,
                            GeslachtId = '-',
                            UserId = user.Id,
                            Deleted = DateTime.MaxValue
                        },

                        new Student
                        {
                            Voornaam = "-",
                            Achternaam = "-",
                            Geboortedatum = DateTime.Now,
                            GeslachtId = '-',
                            UserId = user.Id,
                            Deleted = DateTime.Now
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

                if (user != null)
                {
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string> { RoleId = "Beheerder", UserId = user.Id},
                        new IdentityUserRole<string> { RoleId = "Docent", UserId = user.Id},
                        new IdentityUserRole<string> { RoleId = "Student", UserId = user.Id }
                        );
                    context.SaveChanges();
                }
            }
        }
    }
}
