using LicenseManager.Data;
using LicenseManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LicenseManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LicenseValidationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LicenseValidationController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Validates a license key.
    /// </summary>
    /// <param name="key">The license key to validate.</param>
    /// <returns>Validation result with license details.</returns>
    [HttpGet("validate")]
    public async Task<IActionResult> Validate([FromQuery] string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return BadRequest(new { valid = false, message = "License key is required." });
        }

        var license = await _context.Licenses
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Key == key);

        if (license == null)
        {
            return NotFound(new { valid = false, message = "License key not found." });
        }

        if (license.Status == LicenseStatus.Revoked)
        {
            return Ok(new { valid = false, message = "License has been revoked.", licenseType = license.Type.ToString() });
        }

        if (license.Status == LicenseStatus.Expired || (license.ExpiresAt.HasValue && license.ExpiresAt.Value < DateTime.UtcNow))
        {
            return Ok(new { valid = false, message = "License has expired.", licenseType = license.Type.ToString() });
        }

        return Ok(new
        {
            valid = true,
            message = "License is valid.",
            licenseType = license.Type.ToString(),
            email = license.Email,
            expiresAt = license.ExpiresAt
        });
    }
}
