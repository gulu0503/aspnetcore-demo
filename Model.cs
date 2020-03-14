using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_demo
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options):base(options){}
        public DbSet<Product> Products { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
}