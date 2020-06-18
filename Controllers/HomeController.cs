using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using ProductsAndCategories.Models;
using Microsoft.AspNetCore.Http;

namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
    
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
    
        [HttpGet("")]
        public IActionResult Index()
        {
            return Redirect("/products");
        }

        [HttpGet("products")]
        public IActionResult Products()
        {

            ViewBag.Products = dbContext.Products.ToList();
            return View();

        }

        [HttpPost("products/create")]
        public IActionResult CreateProduct(Product newProduct)
        {
          if(ModelState.IsValid)
          {
            dbContext.Add(newProduct);
            dbContext.SaveChanges();
            return Redirect("/products");
          }
          ViewBag.Products = dbContext.Products.ToList();
          return View("Products");
        }

        [HttpGet("products/{productId}")]
        public IActionResult DisplayProduct(int productId)
        {
          Product product = dbContext.Products
                                      .Include(c => c.AssociatedCategories)
                                      .ThenInclude(a => a.Category)
                                      .FirstOrDefault(p => p.ProductId == productId);
          List<Category> current = new List<Category>();
          foreach (Association a in product.AssociatedCategories)
          {
              current.Add(a.Category);
          }
          ViewBag.Categories = dbContext.Categories.Except(current);
          return View(product);
        }

        [HttpPost("products/{productId}/add")]
        public IActionResult AddCategory(int productId, int categoryId)
        {
          Association newAssociation = new Association();
          newAssociation.ProductId = productId;
          newAssociation.CategoryId = categoryId;
          dbContext.Add(newAssociation);
          dbContext.SaveChanges();
          return Redirect($"/products/{productId}");
        }


        [HttpGet("categories")]
        public IActionResult Categories()
        {
          ViewBag.Categories = dbContext.Categories.ToList();
          return View();
        }

        [HttpPost("categories/create")]
        public IActionResult CreateCategory(Category newCategory)
        {
          if(ModelState.IsValid)
          {
            dbContext.Add(newCategory);
            dbContext.SaveChanges();
            return Redirect("/categories");
          }
          ViewBag.Categories = dbContext.Categories.ToList();
          return View("Categories");
        }

        [HttpGet("categories/{categoryId}")]
        public IActionResult DisplayCategory(int categoryId)
        {
          Category category = dbContext.Categories
                                      .Include(c => c.AssociatedProducts)
                                      .ThenInclude(a => a.Product)
                                      .FirstOrDefault(c => c.CategoryId == categoryId);
          List<Product> current = new List<Product>();
          foreach (Association a in category.AssociatedProducts)
          {
              current.Add(a.Product);
          }
          ViewBag.Products = dbContext.Products.Except(current);
          return View(category);
        }
                
        [HttpPost("categories/{categoryId}/add")]
        public IActionResult AddProduct(int productId, int categoryId)
        {
          Association newAssociation = new Association();
          newAssociation.ProductId = productId;
          newAssociation.CategoryId = categoryId;
          dbContext.Add(newAssociation);
          dbContext.SaveChanges();
          return Redirect($"/categories/{categoryId}");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
