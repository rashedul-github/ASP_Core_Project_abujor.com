using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abujor_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abujor_Project.Controllers
{
    public class ViewController : Controller
    {
        private readonly AbujorDbContext db;
        public ViewController(AbujorDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var data = await db.Categories.Include(n => n.Products).ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> ProductView(int id)
        {
            var data=await db.Products.Where(n => n.CategoryId==id).ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> proDetail(int id)
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
        public async Task<IActionResult> viewAllProducts()
        {
            var data = await db.Products.Include(n=>n.Category).Include(v => v.ProductDetail).ToListAsync();
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> viewAllProducts(string postModel)
        {
            if (postModel != null)
            {
                var postData = await db.Products.Include(m=>m.Category).Where(p => p.ProductModel.Contains(postModel)).ToListAsync();
                return View(postData);
            }
            else
            {
                return View(await db.Products.Include(m => m.Category).ToListAsync());
            }
            
        }

        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term =HttpContext.Request.Query["term"].ToString();
                var postModel = await db.Products.Where(p => p.ProductModel.Contains(term))
                                        .Select(p => p.ProductModel).ToListAsync();
                return Ok(postModel);
            }
            catch
            {

                return BadRequest();
            }
        }
    }
}