using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DOINHE.Entitys
{
    public class User
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? Password { get; set; }

        public string? UserName { get; set; }

        public string? Phone { get; set; }

        public int? Role { get; set; }

        public double? Money { get; set;}
        public virtual ICollection<Order> Orders { get; set; }
    }
}
