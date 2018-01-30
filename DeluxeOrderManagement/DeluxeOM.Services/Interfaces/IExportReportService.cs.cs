using DeluxeOM.Models;
using DeluxeOM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeluxeOM.Services
{
    interface IExportReportService
    {
        DownLoadFile GenerateCancelAvailsReport(ReportSearch reportSearch);
        DownLoadFile GenerateFinanceReport(ReportSearch reportSearch);
        DownLoadFile GenerateAnnouncementAnalysisReport(ReportSearch reportSearch);
        DownLoadFile GenerateOrderReport(OrderSearch searchOrder);
        List<Customer> GetCustomers();
        DownLoadFile GenerateAnnouncementChangeReport(ReportSearch reportSearch);
        ReportSearch GetSearchValues();

    }
}
