using Microsoft.AspNetCore.Identity;

namespace Studentenbeheer.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public string? Voornaam { get; set; }
    public string? Achternaam { get; set; }
}

public class AppUserViewModel
{
    public string Id { get; set; }
    public string Voornaam { get; set; }
    public string Achternaam { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool Lockout { get; set; }
    public bool Student { get; set; }
    public bool Docent { get; set; }
    public bool Beheerder { get; set; }
}
