using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DOINHE.Entitys
{
    public class User
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? UserName { get; set; }

        public string? Phone { get; set; }

        public int? Role { get; set; }

        public double? Money { get; set;}
        public virtual ICollection<Order> Orders { get; set; }
    }
}
