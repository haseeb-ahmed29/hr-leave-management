using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrLeaveManagement.Data;
using HrLeaveManagement.Models;

namespace HrLeaveManagement.Controllers;
public class EmployeesController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, string? status)
    {
        var query = db.Employees.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Name.Contains(search));
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
        ViewBag.Search = search; ViewBag.Status = status;
        return View(await query.OrderByDescending(x => x.CreatedAt).ToListAsync());
    }
    public IActionResult Create() => View(new Employee());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee item)
    { if (!ModelState.IsValid) return View(item); db.Employees.Add(item); await db.SaveChangesAsync(); TempData["Notice"] = "Record created successfully."; return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int? id) => id is null ? NotFound() : (await db.Employees.FindAsync(id) is Employee item ? View(item) : NotFound());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee item)
    { if (id != item.Id) return NotFound(); if (!ModelState.IsValid) return View(item); db.Update(item); await db.SaveChangesAsync(); TempData["Notice"] = "Record updated successfully."; return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Delete(int? id) => id is null ? NotFound() : (await db.Employees.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id) is Employee item ? View(item) : NotFound());
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) { var item = await db.Employees.FindAsync(id); if (item is not null) { db.Employees.Remove(item); await db.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }
}
