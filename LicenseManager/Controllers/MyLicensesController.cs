using LicenseManager.Data;
using LicenseManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LicenseManager.Controllers;

[Authorize]
public class MyLicensesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MyLicensesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: MyLicenses
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var licenses = await _context.Licenses
            .Where(l => l.Email == user.Email)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();

        return View(licenses);
    }

    // GET: MyLicenses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var license = await _context.Licenses
            .FirstOrDefaultAsync(l => l.Id == id && l.Email == user.Email);

        if (license == null) return NotFound();

        return View(license);
    }
}
