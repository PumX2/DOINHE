using DOINHE.Entitys;

namespace DOINHE.DTO
{
    public class ReportDTO
    {
        public int? Id { get; set; }
        public int OrderId { get; set; }

        public IFormFile ImgReport { get; set; }

        public string Description { get; set; }
        public virtual Order? Order { get; set; }
    }
}
