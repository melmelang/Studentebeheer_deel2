using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Models;

namespace Studentenbeheer.Data
{
    public class DatabaseSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new StudentenbeheerContext(
                serviceProvider.GetRequiredService<DbContextOptions<StudentenbeheerContext>>()))
            {
                context.Database.EnsureCreated();

                if (context.Gender.Any() || context.Student.Any())
                {
                    return;
                }

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

                context.Student.AddRange(

                    new Student
                    {
                        Voornaam = "Ine",
                        Achternaam = "DeBast",
                        Geboortedatum = DateTime.Now,
                        GeslachtId = 'V',
                        Deleted = DateTime.MaxValue
                    },

                    new Student
                    {
                        Voornaam = "Antoine",
                        Achternaam = "Couck",
                        Geboortedatum = DateTime.Now,
                        GeslachtId = 'M',
                        Deleted = DateTime.MaxValue
                    },

                    new Student
                    {
                        Voornaam = "Melvin",
                        Achternaam = "Angeli",
                        Geboortedatum = DateTime.Now,
                        GeslachtId = '-',
                        Deleted = DateTime.MaxValue
                    },

                    new Student
                    {
                        Voornaam = "-",
                        Achternaam = "-",
                        Geboortedatum = DateTime.Now,
                        GeslachtId = '-',
                        Deleted = DateTime.Now
                    }

                );
                context.SaveChanges();

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
        }
    }
}
