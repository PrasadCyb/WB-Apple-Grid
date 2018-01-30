using DeluxeOM.Models;
using DeluxeOM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using DeluxeOM.Services.Services;
using DeluxeOM.Utils.Config;
using System.Configuration;

namespace DeluxeOM.WebUI.Controllers
{
    [DeluxeOMAuthorize(Roles = "ReportAdmin")]
    public class TitleController : BaseController
    {
        // GET: Title
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Titles(TitleSearch titleSearch, int? pageNumber)
        {
            try
            {
                TitleService _title = new TitleService();
                var titles = _title.GetTitles().ToPagedList(pageNumber ?? 1, 10);
                var searchValues = _title.GetSearchValues();
                searchValues.SelectedTitleList = searchValues.SelectedTitles.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.TerritoryList = searchValues.Territories.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.LanguageList = searchValues.Languages.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.MPMList = searchValues.MPMs.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.VendorIdList = searchValues.VendorIds.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.VideoVersionList = searchValues.VideoVersions.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                Titles title = new Titles();
                title.TitleSearch = searchValues;
                title.TitleList = titles;
                return View(title);
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult SearchTitle(TitleSearch titleSearch, int? pageNumber)
        {
            if (!ModelState.IsValid)
            {
                TitleService _title = new TitleService();
                var titles = new List<TitleList>().ToPagedList(1,10);
                var searchValues = _title.GetSearchValues();
                searchValues.SelectedTitleList = searchValues.SelectedTitles.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.TerritoryList = searchValues.Territories.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.LanguageList = searchValues.Languages.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.MPMList = searchValues.MPMs.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.VendorIdList = searchValues.VendorIds.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.VideoVersionList = searchValues.VideoVersions.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                Titles title = new Titles();
                title.TitleList = titles;
                title.TitleSearch = titleSearch;
                title.TitleSearch.ContentDistributors = searchValues.ContentDistributors;
                title.TitleSearch.ContentProviders = searchValues.ContentProviders;
                title.TitleSearch.SelectedTitleList = searchValues.SelectedTitleList;
                title.TitleSearch.EditTypes = searchValues.EditTypes;
                title.TitleSearch.TerritoryList = searchValues.TerritoryList;
                title.TitleSearch.LanguageList = searchValues.LanguageList;
                title.TitleSearch.VideoVersionList = searchValues.VideoVersionList;
                title.TitleSearch.MPMList = searchValues.MPMList;
                title.TitleSearch.VendorIdList = searchValues.VendorIdList;
                title.TitleSearch.ComponentTypes = searchValues.ComponentTypes;
                title.TitleSearch.SortOrderList = searchValues.SortOrderList;
                title.TitleSearch.SortByList = searchValues.SortByList;
                return View("Titles", title);
            }
            try
            {
                TitleService _title = new TitleService();
                var titles = _title.SearchTitles(titleSearch).ToPagedList(pageNumber ?? 1, 10);
                var searchValues = _title.GetSearchValues();
                searchValues.SelectedTitleList = searchValues.SelectedTitles.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.TerritoryList = searchValues.Territories.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.LanguageList = searchValues.Languages.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.MPMList = searchValues.MPMs.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.VendorIdList = searchValues.VendorIds.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                searchValues.VideoVersionList = searchValues.VideoVersions.Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x
                }).Distinct().ToList();
                Titles title = new Titles();
                title.TitleList = titles;
                title.TitleSearch = titleSearch;
                title.TitleSearch.ContentDistributors = searchValues.ContentDistributors;
                title.TitleSearch.ContentProviders = searchValues.ContentProviders;
                title.TitleSearch.SelectedTitleList = searchValues.SelectedTitleList;
                title.TitleSearch.EditTypes = searchValues.EditTypes;
                title.TitleSearch.TerritoryList = searchValues.TerritoryList;
                title.TitleSearch.LanguageList = searchValues.LanguageList;
                title.TitleSearch.VideoVersionList = searchValues.VideoVersionList;
                title.TitleSearch.MPMList = searchValues.MPMList;
                title.TitleSearch.VendorIdList = searchValues.VendorIdList;
                title.TitleSearch.ComponentTypes = searchValues.ComponentTypes;
                title.TitleSearch.SortOrderList = searchValues.SortOrderList;
                title.TitleSearch.SortByList = searchValues.SortByList;
                return View("Titles", title);
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public FileResult ExportTitle(TitleSearch titleSearch)
        {
            ExportReportService _export = new ExportReportService();
            DownLoadFile downloadFile = _export.GenerateTitleReport(titleSearch);
            //string path = ConfigurationManager.AppSettings["directoryPath"];
            //string path = App.Config.ReportDirectoryPath;
            //string downloadFile = string.Format(path, file);
            return File(downloadFile.bufferByte, "application/vnd.ms-excel", downloadFile.FileName);
        }
    }
}