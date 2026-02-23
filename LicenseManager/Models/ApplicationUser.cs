using Microsoft.AspNetCore.Identity;

namespace LicenseManager.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public ICollection<License> Licenses { get; set; } = new List<License>();
}
