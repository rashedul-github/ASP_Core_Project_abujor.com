using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Abujor_Project.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category name is required."), StringLength(40), Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Category image is required."), StringLength(250), Display(Name = "Image")]
        public string Image { get; set; }
        //
        public ICollection<Product> Products { get; set; }
    }
    public class Product
    {
        public Product()
        {
            this.ProductDetail = new HashSet<ProductDetail>();
            this.Orders = new HashSet<Order>();
        }
        public int ProductId { get; set; }
        [Required(ErrorMessage ="Product model is required."),StringLength(40),Display(Name = "Product Model")]
        public string ProductModel { get; set; }
        [Required(ErrorMessage = "Product price is required."), Display(Name = "Product Price")]
        [Column(TypeName ="money"),DisplayFormat(DataFormatString = "{0:0.00}",ApplyFormatInEditMode =true)]
        public decimal ProductPrice { get; set; }
        public bool Status { get; set; }
        [Required(ErrorMessage = "Picture is required."), StringLength(250), Display(Name = "Picture")]
        public string Picture { get; set; }
        //
        [Required]
        public int CategoryId { get; set; }
        //
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        //
        public ICollection<ProductDetail> ProductDetail { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }
        [Required(ErrorMessage = "Product size is required."), StringLength(40)]
        public string Size { get; set; }
        [Required(ErrorMessage = "Product quantity is required.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Product color is required."), StringLength(40)]
        public string Color { get; set; }
        [Required(ErrorMessage = "Product decr. is required."), StringLength(200)]
        public string Description { get; set; }
        //
        [Required]
        public int ProductId { get; set; }
        //
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }
    public class Order
    {
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Order date is required."), Display(Name = "Order Date*")]
        [Column(TypeName ="date"),DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}",ApplyFormatInEditMode =true)]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Customer name is required."), StringLength(40), Display(Name = "Customer Name*")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Customer number is required."), Display(Name = "Customer Phone*")]
        public int CustomerPhone { get; set; }
        [StringLength(40), Display(Name = "Customer Email")]
        public string CustomerEmail { get; set; }
        [Required(ErrorMessage = "Customer address is required."), StringLength(200), Display(Name = "Customer Address*")]
        public string Address { get; set; }
        //
        [Required]
        public int ProductId { get; set; }
        //
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }

    public class AbujorDbContext : DbContext
    {
        //ctor
        public AbujorDbContext(DbContextOptions<AbujorDbContext> options):base(options){}
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
