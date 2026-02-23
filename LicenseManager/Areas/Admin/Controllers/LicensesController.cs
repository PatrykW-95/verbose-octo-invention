using LicenseManager.Data;
using LicenseManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LicenseManager.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class LicensesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public LicensesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Admin/Licenses
    public async Task<IActionResult> Index()
    {
        var licenses = await _context.Licenses
            .Include(l => l.User)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
        return View(licenses);
    }

    // GET: Admin/Licenses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var license = await _context.Licenses
            .Include(l => l.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (license == null) return NotFound();

        return View(license);
    }

    // GET: Admin/Licenses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Licenses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Type,Email,ExpiresAt,Notes")] License license)
    {
        if (ModelState.IsValid)
        {
            license.Key = Guid.NewGuid().ToString();
            license.CreatedAt = DateTime.UtcNow;
            license.Status = LicenseStatus.Active;

            var user = await _userManager.FindByEmailAsync(license.Email);
            if (user != null)
            {
                license.UserId = user.Id;
            }

            _context.Add(license);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(license);
    }

    // GET: Admin/Licenses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var license = await _context.Licenses.FindAsync(id);
        if (license == null) return NotFound();

        return View(license);
    }

    // POST: Admin/Licenses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Key,Type,Status,Email,ExpiresAt,Notes")] License license)
    {
        if (id != license.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existing = await _context.Licenses.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
                if (existing == null) return NotFound();

                license.CreatedAt = existing.CreatedAt;

                var user = await _userManager.FindByEmailAsync(license.Email);
                license.UserId = user?.Id;

                _context.Update(license);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Licenses.Any(e => e.Id == license.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(license);
    }

    // GET: Admin/Licenses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var license = await _context.Licenses
            .Include(l => l.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (license == null) return NotFound();

        return View(license);
    }

    // POST: Admin/Licenses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var license = await _context.Licenses.FindAsync(id);
        if (license != null)
        {
            _context.Licenses.Remove(license);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
