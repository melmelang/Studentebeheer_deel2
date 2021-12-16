using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Data;

public class AppDataContext : IdentityDbContext<AppUser>
{
    public AppDataContext(DbContextOptions<AppDataContext> options)
        : base(options)
    {
    }

    public DbSet<Studentenbeheer.Models.Student> Student { get; set; }

    public DbSet<Studentenbeheer.Models.Gender> Gender { get; set; }

    public DbSet<Studentenbeheer.Models.Module> Module { get; set; }

    public DbSet<Studentenbeheer.Models.Inschrijvingen> Inschrijvingen { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

}
