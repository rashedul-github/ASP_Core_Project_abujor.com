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
    public class CategoriesController : Controller
    {
        private readonly AbujorDbContext db;
        private readonly IHostingEnvironment env;
        public CategoriesController(AbujorDbContext db, IHostingEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            var data = await db.Categories.ToListAsync();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryVM v, IFormFile Image)
        {
            try
            {
                string ext = Path.GetExtension(Image.FileName);
                string f = Guid.NewGuid() + ext;
                if (!Directory.Exists(env.WebRootPath + "\\uploads\\"))
                {
                    Directory.CreateDirectory(env.WebRootPath + "\\uploads\\");
                }
                using (FileStream filestream = System.IO.File.Create(env.WebRootPath + "\\uploads\\" + f))
                {
                    Image.CopyTo(filestream);
                    filestream.Flush();
                    Category c = new Category();
                    c.CategoryId = v.CategoryId;
                    c.CategoryName = v.CategoryName;
                    c.Image = "/uploads/" + f;
                    await db.AddAsync(c);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            var data = await db.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryVM v, IFormFile Image)
        {
            try
            {
                string ext = Path.GetExtension(Image.FileName);
                string f = Guid.NewGuid() + ext;
                if (!Directory.Exists(env.WebRootPath + "\\uploads\\"))
                {
                    Directory.CreateDirectory(env.WebRootPath + "\\uploads\\");
                }
                using (FileStream filestream = System.IO.File.Create(env.WebRootPath + "\\uploads\\" + f))
                {
                    Image.CopyTo(filestream);
                    filestream.Flush();
                    Category c = new Category();
                    c.CategoryId = v.CategoryId;
                    c.CategoryName = v.CategoryName;
                    c.Image = "/uploads/" + f;
                    db.Entry(c).State = EntityState.Modified;
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
            var data = await db.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            return View(data);
        }
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                Category c = new Category { CategoryId = id };
                db.Entry(c).State = EntityState.Deleted;
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