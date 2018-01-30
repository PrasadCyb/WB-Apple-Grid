using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;
using DeluxeOM.Services;
using DeluxeOM.Services.Services;
using DeluxeOM.Models;
using System.IO;
using DeluxeOM.Utils.Config;

namespace DeluxeOM.WebUI.Controllers
{
    [DeluxeOMAuthorize(Roles = "ReportAdmin")]
    public class ReportController : BaseController
    {


        public ActionResult Reports(ReportSearch reportSearch)
        {
            ReportsModel reportModel = new ReportsModel();
            ExportReportService _report = new ExportReportService();
            //reportModel.Name = Session["Name"].ToString();
            var searchValues = _report.GetSearchValues();
            reportModel.ReportSearch.ContentDistributors = searchValues.ContentDistributors;
            reportModel.ReportSearch.ContentProviders = searchValues.ContentProviders;
            reportModel.ReportSearch.LocalEdits = searchValues.LocalEdits;
            reportModel.ReportSearch.AnnouncementProcessedDate = searchValues.AnnouncementProcessedDate;

            return View(reportModel);
        }

        public FileResult Reports1(ReportSearch reportSearch)
        {
            string path = string.Empty;
            string downloadFile = string.Empty;
            DownLoadFile downLoadFile=null;

            ExportReportService exportService = new ExportReportService();

            if (reportSearch.ReportTitleID.Equals(1))
            {
                downLoadFile = exportService.GenerateAnnouncementAnalysisReport(reportSearch);
            }

            else if (reportSearch.ReportTitleID.Equals(2))
            {
                downLoadFile = exportService.GenerateCancelAvailsReport(reportSearch);
            }
            else if (reportSearch.ReportTitleID.Equals(3))
            {
                downLoadFile = exportService.GenerateFinanceReport(reportSearch);
            }
            else if (reportSearch.ReportTitleID.Equals(4))
            {
                downLoadFile = exportService.GenerateAnnouncementChangeReport(reportSearch);
            }

            //file = TempData["fileName"].ToString();
            //path = ConfigurationManager.AppSettings["directoryPath"];
            //path = App.Config.ReportDirectoryPath;
            //string fullPath = string.Format(path, file);
            return File(downLoadFile.bufferByte, "application/vnd.ms-excel", downLoadFile.FileName);
            //TempData["fileName"] = file;
            //return RedirectToAction("Reports");
        }

        public virtual ActionResult Download()
        {
            string file = string.Empty;
            if (!string.IsNullOrEmpty(TempData["fileName"].ToString()))
            {
                file = TempData["fileName"].ToString();
                //string path = ConfigurationManager.AppSettings["directoryPath"];
                string path = App.Config.ReportDirectoryPath;
                string fullPath = string.Format(path, file);
                return File(fullPath, "application/vnd.ms-excel", file);
            }
            else
            {
                return View();
            }
        }
    }
}