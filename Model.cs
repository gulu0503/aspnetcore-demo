using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_demo
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=product.db");
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
}