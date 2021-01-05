using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Abujor_Project.Models.VM
{
    public class ProductVM
    {
        
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product model is required."), Display(Name = "Product Model*")]
        public string ProductModel { get; set; }
        [Required(ErrorMessage = "Product price is required."), Display(Name = "Product Price*")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ProductPrice { get; set; }
        public bool Status { get; set; }
        [Required(ErrorMessage = "Picture is required."), Display(Name = "Picture")]
        public IFormFile Picture { get; set; }
        //
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
