using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Models;

namespace Studentenbeheer.Data
{
    public class StudentenbeheerContext : DbContext
    {
        public StudentenbeheerContext (DbContextOptions<StudentenbeheerContext> options)
            : base(options)
        {
        }

        public DbSet<Studentenbeheer.Models.Student> Student { get; set; }

        public DbSet<Studentenbeheer.Models.Gender> Gender { get; set; }

        public DbSet<Studentenbeheer.Models.Module> Module { get; set; }

        public DbSet<Studentenbeheer.Models.Inschrijvingen> Inschrijvingen { get; set; }
    }
}
