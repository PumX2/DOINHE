using DOINHE_BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DOINHE_DataAccess
{
    public class ReportDAO
    {
        public static List<Report> GetReports()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Reports.ToList();
            }
        }

        public static Report GetReportById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Reports.FirstOrDefault(r => r.Id == id);
            }
        }

        public static void InsertReport(Report report)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Reports.Add(report);
                context.SaveChanges();
            }
        }

        public static void UpdateReport(Report report)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Reports.Update(report);
                context.SaveChanges();
            }
        }

        public static void DeleteReport(Report report)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Reports.Remove(report);
                context.SaveChanges();
            }
        }
    }
}
