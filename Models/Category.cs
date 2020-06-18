using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace ProductsAndCategories.Models
    {
        public class Category
        {
            [Key]
            public int CategoryId { get; set; }

            [Required]
            [Display(Name = "Name:")]
            public string Name { get; set; }

            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;

            public List<Association> AssociatedProducts {get; set;}
        }
    }