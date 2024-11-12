using DOINHE_BusinessObject;
using System.Collections.Generic;

namespace DOINHE_Repository
{
    public interface IReportRepository
    {
        List<Report> GetAllReports();
        Report GetReportById(int id);
        void SaveReport(Report report);
        void UpdateReport(Report report);
        void DeleteReport(Report report);
    }
}
