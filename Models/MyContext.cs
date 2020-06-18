using Microsoft.EntityFrameworkCore; 
    namespace ProductsAndCategories.Models
    {
        public class MyContext : DbContext
        {
            public MyContext(DbContextOptions options) : base(options) { }
            
						// This is where the models go
            public DbSet<Product> Products {get;set;}
            public DbSet<Category> Categories {get;set;}

            public DbSet<Association> Associations {get;set;}
        }
    }