using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using GraniteHouse.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {

        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ProductsViewModel ProductsVM { get; set; }

        public ProductsController(ApplicationDbContext db) {
         _db = db;

         ProductsVM = new ProductsViewModel(){
             ProductTypes = _db.ProductTypes.ToList(),
             SpecialTags = _db.SpecialTags.ToList(),
             Products = new Models.Products()
         };
        }
            

        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(m => m.ProductTypes).Include(m=>m.SpecialTags);
            return View(await products.ToListAsync());
        }

        //GET Create Action Method
        public IActionResult Create() => View();

        //POST Create action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products)
        {
            if (ModelState.IsValid)
            {
                _db.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }


        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST Edit action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Products products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //POST Delete action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _db.Products.FindAsync(id);
            _db.Products.Remove(products);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}