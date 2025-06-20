using System;
using Core_Assignment_2.Data;
using Core_Assignment_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Core_Assignment_2.Controllers
{
    public class InventoryController : Controller
    {
        private readonly InventoryDBContext _context;

        public InventoryController(InventoryDBContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index(string searchString, string categoryFilter)
        {
            var categories = await _context.Inventories
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            var products = from p in _context.Inventories select p;

            if (!string.IsNullOrEmpty(searchString))
                products = products.Where(p => p.Name.Contains(searchString));

            if (!string.IsNullOrEmpty(categoryFilter))
                products = products.Where(p => p.Category == categoryFilter);

            ViewBag.Categories = new SelectList(categories);
            return View(await products.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryModel product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Inventories.FindAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InventoryModel product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Inventories.Any(p => p.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _context.Inventories
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (product != null)
            {
                _context.Inventories.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Inventories.FindAsync(id);
            if (product != null)
                _context.Inventories.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Inventories.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }
    }
}
