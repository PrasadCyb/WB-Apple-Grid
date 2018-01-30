using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeluxeOM.Services;
using PagedList;
using DeluxeOM.Models;
using DeluxeOM.Services.Services;
using DeluxeOM.Utils.Config;
using System.Configuration;
using System.IO;

namespace DeluxeOM.WebUI.Controllers
{
    [DeluxeOMAuthorize(Roles = "ReportAdmin")]
    public class OrderController : BaseController
    {
        public readonly int lockExpiryMin=App.Config.OrderUnlockPeriod;
        // GET: Oder
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Orders(OrderSearch orderSearch, int? pageNumber)
        {
            try
            {
                OrderService _order = new OrderService();
                OrderSearch searchValues = _order.GetSearchValues();

                //to persist values between pages
                OrderSearch persistedValues = TempData.Peek("orderSearch") as OrderSearch;
                if (persistedValues != null)
                {
                    searchValues.SelectedTitle = persistedValues.SelectedTitle;
                    searchValues.ContentProvider = persistedValues.ContentProvider;
                    searchValues.ContentDistributor = persistedValues.ContentDistributor; ;
                    searchValues.EditType = persistedValues.EditType;
                    searchValues.LocalEdit = persistedValues.LocalEdit;
                    searchValues.OrderStatus = persistedValues.OrderStatus;
                    searchValues.GreenLightSent = persistedValues.GreenLightSent;
                    searchValues.GreenLightReceived = persistedValues.GreenLightReceived;
                    searchValues.Territory = persistedValues.Territory;
                    searchValues.MediaType = persistedValues.MediaType;
                    searchValues.StartDate = persistedValues.StartDate;
                    searchValues.EndDate = persistedValues.EndDate;
                    searchValues.SortBy = persistedValues.SortBy;
                    searchValues.SortOrder = persistedValues.SortOrder;
                }
                var orders = _order.SearchOrders(searchValues).ToPagedList(pageNumber ?? 1, 10);

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
                OrderMgt orderMgt = new OrderMgt();
                orderMgt.orders = orders;
                orderMgt.OrderSearch = searchValues;
                if (TempData["SavedStatus"] != null)
                {
                    orderMgt.SavedStatus = Convert.ToBoolean(TempData["SavedStatus"]);
                }
                orderMgt.UserId = CurrentUser.UserId;
                orderMgt.OrderUnlockPeriod = lockExpiryMin;
                return View(orderMgt);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult SearchOrder(OrderSearch orderSearch, int? pageNumber)
        {
            OrderService _order = new OrderService();
            try
            {
                var orders = _order.SearchOrders(orderSearch).ToPagedList(pageNumber ?? 1, 10);

                TempData["orderSearch"] = orderSearch;
                OrderSearch searchValues = _order.GetSearchValues();

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
                OrderMgt orderMgt = new OrderMgt();
                orderMgt.orders = orders;
                orderMgt.OrderSearch = orderSearch;
                orderMgt.OrderSearch.ContentDistributors = searchValues.ContentDistributors;
                orderMgt.OrderSearch.ContentProviders = searchValues.ContentProviders;
                orderMgt.OrderSearch.SelectedTitleList = searchValues.SelectedTitleList;
                orderMgt.OrderSearch.EditTypes = searchValues.EditTypes;
                orderMgt.OrderSearch.LocalEdits = searchValues.LocalEdits;
                orderMgt.OrderSearch.OrderStatuses = searchValues.OrderStatuses;
                orderMgt.OrderSearch.MediaTypes = searchValues.MediaTypes;
                orderMgt.OrderSearch.GreenLights = searchValues.GreenLights;
                orderMgt.OrderSearch.GreenLights = searchValues.GreenLights;
                orderMgt.OrderSearch.TerritoryList = searchValues.TerritoryList;
                orderMgt.OrderSearch.SortByList = searchValues.SortByList;
                orderMgt.OrderSearch.SortOrderList = searchValues.SortOrderList;
                orderMgt.UserId = CurrentUser.UserId;
                orderMgt.OrderUnlockPeriod = lockExpiryMin;
                return View("Orders", orderMgt);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [DeluxeOMAuthorize(Roles = "ManagerAdmin")]
        [HttpGet]
        public ActionResult EditOrder(int id, int annId)
        {
            try
            {
                OrderSearch persistedValues = TempData.Peek("orderSearch") as OrderSearch;

                OrderService _order = new OrderService();
                var order = _order.SearchEditOrder(id, annId);
                if (order.IsLocked == true && order.LockedBy != CurrentUser.UserId)
                {
                    if (order.LockedOn != null && order.LockedOn.Value.AddMinutes(lockExpiryMin) < DateTime.UtcNow)
                    {
                        order.IsAlloweToEdit = true;
                        _order.LockOrder(id, CurrentUser.UserId);
                    }
                    else
                    {
                        order.IsAlloweToEdit = false;
                    }
                }
                else
                {
                    order.IsAlloweToEdit = true;
                    _order.LockOrder(id, CurrentUser.UserId);
                }
                return View(order);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder(Order order)
        {
            try
            {
                
                OrderService _order = new OrderService();
                bool status = _order.EditOrder(order);
                _order.UnlockOrder(order.OrderId,CurrentUser.UserId);
                TempData["SavedStatus"] = status;
                OrderSearch orderSearch = TempData.Peek("orderSearch") as OrderSearch;
                return RedirectToAction("SearchOrder", orderSearch);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult CancelEdit(Order order)
        {
            try
            {
                OrderService _order = new OrderService();
                _order.UnlockOrder(order.OrderId, CurrentUser.UserId);
                OrderSearch orderSearch = TempData.Peek("orderSearch") as OrderSearch;
                return RedirectToAction("SearchOrder", orderSearch);
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        [DeluxeOMAuthorize(Roles = "ManagerAdmin")]
        [HttpGet]
        public ActionResult CreateOrder()
        {
            OrderSearch persistedValues = TempData.Peek("orderSearch") as OrderSearch;

            OrderService _order = new OrderService();
            Order order = new Order();
            var dropDownValues = _order.GetDropDownValue();
            order.Territories = dropDownValues.Territory.Select(x => new SelectListItem()
            {
                Text = x.TerritoryName,
                Value = x.TerritoryID.ToString()
            }).Distinct().ToList();

            order.Languages = dropDownValues.Language.Select(x => new SelectListItem()
            {
                Text = x.LanguageName,
                Value = x.LanguageID.ToString()
            }).Distinct().ToList();

            order.Titles = dropDownValues.Title.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Name
            }).Distinct().ToList();
            order.Assets = dropDownValues.Assets;
            order.OrderCategories = dropDownValues.orderCategory;
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(Order saveOrder)
        {
            OrderService _order = new OrderService();
            var orderStatus = _order.CreateOrder(saveOrder);
            OrderSearch orderSearch = TempData.Peek("orderSearch") as OrderSearch;
            if (orderStatus.IsSaved)
            {
                TempData["SavedStatus"] = orderStatus.IsSaved;
                
                return RedirectToAction("Orders", orderSearch);
            }
            else
            {
                Order order = new Order();
                var dropDownValues = _order.GetDropDownValue();
                order.Territories = dropDownValues.Territory.Select(x => new SelectListItem()
                {
                    Text = x.TerritoryName,
                    Value = x.TerritoryID.ToString()
                }).Distinct().ToList();

                order.Languages = dropDownValues.Language.Select(x => new SelectListItem()
                {
                    Text = x.LanguageName,
                    Value = x.LanguageID.ToString()
                }).Distinct().ToList();

                order.Titles = dropDownValues.Title.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name
                }).Distinct().ToList();
                order.Assets = dropDownValues.Assets;
                order.OrderCategories = dropDownValues.orderCategory;
                ModelState.AddModelError("", orderStatus.ErrorMessage);
                
                return View(order);
            }
        }

        [HttpGet]
        public FileResult ExportOrder(OrderSearch orderSearch)
        {
            ExportReportService _export = new ExportReportService();
            DownLoadFile downloadFile = _export.GenerateOrderReport(orderSearch);
            //string path = ConfigurationManager.AppSettings["directoryPath"];
            string path = App.Config.ReportDirectoryPath;
            //string downloadFile = string.Format(path, file);
            ////Stream fs = File.OpenRead(@"c:\testdocument.docx");
            //FileStream stream = new FileStream(downloadFile, FileMode.Open, FileAccess.Read);

            //byte[] buffer = new byte[2048];
            //byte[] bufferByte;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, read);
            //    }
            //    bufferByte = ms.ToArray();
            //};
            //stream.Close();
            //_export.DeleteFile(downloadFile);
            return File(downloadFile.bufferByte, "application/vnd.ms-excel", downloadFile.FileName);
        }

    }
}