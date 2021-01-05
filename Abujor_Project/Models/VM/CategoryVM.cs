using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Abujor_Project.Models.VM
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category name is required."), Display(Name = "Category Name*")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Category image is required."), Display(Name = "Image*")]
        public IFormFile Image { get; set; }
       
    }
}
