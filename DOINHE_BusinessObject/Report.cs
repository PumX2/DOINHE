using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOINHE_BusinessObject
{
    public class Report
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }

        public byte[]? ImgReport { get; set; }

        public string? Description { get; set; }
        public virtual Order? Order { get; set; }
    }
}
