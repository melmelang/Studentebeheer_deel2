using Microsoft.AspNetCore.Identity;

namespace Studentenbeheer.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public string? Voornaam { get; set; }
    public string? Achternaam { get; set; }
}
