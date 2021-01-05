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
    public class OrderController : Controller
    {
        private readonly AbujorDbContext db;
        public OrderController(AbujorDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var data = await db.Orders.Include(m => m.Product).ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.GetProduct = await db.Products.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order o)
        {
            try
            {
                await db.AddAsync(o);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.GetProduct = await db.Products.ToListAsync();
            var data = await db.Orders.Include(n => n.Product).FirstOrDefaultAsync(x => x.OrderId== id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Order o)
        {
            try
            {
                db.Entry(o).State = EntityState.Modified;
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
            var data = await db.Orders.Include(n => n.Product).FirstOrDefaultAsync(x => x.OrderId == id);
            return View(data);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            Order o = new Order { OrderId = id };
            db.Entry(o).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}