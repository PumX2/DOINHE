using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DOINHE.Entitys
{
    public class Category
    {  
        public int Id { get; set; } 

        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
