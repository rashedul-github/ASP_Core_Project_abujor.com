using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abujor_Project.Models;
using Abujor_Project.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abujor_Project.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly AbujorDbContext db;
        private readonly IHostingEnvironment env;
        public ProductsController(AbujorDbContext db, IHostingEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            var data = await db.Products.Include(n=>n.Category).ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.GetCategories = await db.Categories.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductVM v, IFormFile Picture)
        {
            try
            {
                string ext = Path.GetExtension(Picture.FileName);
                string f = Guid.NewGuid() + ext;
                if (!Directory.Exists(env.WebRootPath + "\\uploads\\"))
                {
                    Directory.CreateDirectory(env.WebRootPath + "\\uploads\\");
                }
                using (FileStream filestream = System.IO.File.Create(env.WebRootPath + "\\uploads\\" + f))
                {
                    Picture.CopyTo(filestream);
                    filestream.Flush();
                    Product p = new Product();
                    p.ProductId = v.ProductId;
                    p.ProductModel = v.ProductModel;
                    p.ProductPrice = v.ProductPrice;
                    p.Status = v.Status;
                    p.CategoryId = v.CategoryId;
                    p.Picture = "/uploads/" + f;
                    await db.AddAsync(p);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Create", "ProductDetails", new { id = p.ProductId });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.GetCategories = await db.Categories.ToListAsync();
            var data = await db.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM v, IFormFile Picture)
        {
            try
            {
                string ext = Path.GetExtension(Picture.FileName);
                string f = Guid.NewGuid() + ext;
                if (!Directory.Exists(env.WebRootPath + "\\uploads\\"))
                {
                    Directory.CreateDirectory(env.WebRootPath + "\\uploads\\");
                }
                using (FileStream filestream = System.IO.File.Create(env.WebRootPath + "\\uploads\\" + f))
                {
                    Picture.CopyTo(filestream);
                    filestream.Flush();
                    Product p = new Product();
                    p.ProductId = v.ProductId;
                    p.ProductModel = v.ProductModel;
                    p.ProductPrice = v.ProductPrice;
                    p.Status = v.Status;
                    p.CategoryId = v.CategoryId;
                    p.Picture = "/uploads/" + f;
                    db.Entry(p).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var data = await db.Products.Include(m=>m.Category).FirstOrDefaultAsync(x => x.ProductId == id);
            return View(data);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                Product p = new Product { ProductId = id };
                db.Entry(p).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}