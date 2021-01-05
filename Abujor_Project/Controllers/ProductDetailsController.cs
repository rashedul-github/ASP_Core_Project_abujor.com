using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abujor_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abujor_Project.Controllers
{
    [Authorize]
    public class ProductDetailsController : Controller
    {
        private readonly AbujorDbContext db;
        public ProductDetailsController(AbujorDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var data = await db.ProductDetails.Include(m => m.Product).ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> Details(int id)
        {
            var data = await db.ProductDetails.Include(n => n.Product).FirstOrDefaultAsync(x => x.ProductId == id);
            return View(data);
        }
        public async Task<IActionResult> Create(int id)
        {
            ViewBag.ProductId = id;
            ViewBag.GetProduct = await db.Products.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductDetail d)
        {
            try
            {
                await db.AddAsync(d);
                await db.SaveChangesAsync();
                return RedirectToAction("Index","Products");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.GetProduct = await db.Products.ToListAsync();
            var data = await db.ProductDetails.Include(n => n.Product).FirstOrDefaultAsync(x => x.ProductDetailId == id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductDetail d)
        {
            try
            {
                db.Entry(d).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var data = await db.ProductDetails.Include(n => n.Product).FirstOrDefaultAsync(x => x.ProductDetailId == id);
            return View(data);
        }
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            ProductDetail d = new ProductDetail { ProductDetailId = id };
            db.Entry(d).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}