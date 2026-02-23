using System.ComponentModel.DataAnnotations;

namespace LicenseManager.Models;

public enum LicenseType
{
    Free,
    Paid
}

public enum LicenseStatus
{
    Active,
    Expired,
    Revoked
}

public class License
{
    public int Id { get; set; }

    [Required]
    public string Key { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public LicenseType Type { get; set; }

    public LicenseStatus Status { get; set; } = LicenseStatus.Active;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }

    public string? Notes { get; set; }
}
