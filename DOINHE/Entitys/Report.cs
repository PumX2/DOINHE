using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DOINHE.Entitys
{
    public class Report
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public byte[]? ImgReport { get; set; }

        public string Description { get; set; }
        public virtual Order? Order { get; set; }
    }
}
