using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOINHE_BusinessObject
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ProductId { get; set; }
        public int? UserId { get; set; }

        public DateTime? OrderDate { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }

        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
    }

}
