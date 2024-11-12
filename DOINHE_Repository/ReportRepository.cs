using DOINHE_BusinessObject;
using DOINHE_DataAccess;
using System.Collections.Generic;

namespace DOINHE_Repository
{
    public class ReportRepository : IReportRepository
    {
        public List<Report> GetAllReports() => ReportDAO.GetReports();
        public Report GetReportById(int id) => ReportDAO.GetReportById(id);
        public void SaveReport(Report report) => ReportDAO.InsertReport(report);
        public void UpdateReport(Report report) => ReportDAO.UpdateReport(report);
        public void DeleteReport(Report report) => ReportDAO.DeleteReport(report);
    }
}
