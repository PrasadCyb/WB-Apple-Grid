using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeluxeOM.Models;
namespace DeluxeOM.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetOrders(string whrSQLquery)
        {
            var _context = new DeluxeOrderManagementEntities();
            var orders = _context.usp_Get_Orders(whrSQLquery, string.Empty).Select(x =>
                                                          new Order
                                                          {
                                                              AnnouncemntId = x.AnnouncementId,
                                                              OrderId = x.OrderID,
                                                              //VId = x.VId,
                                                              Title = x.Title,
                                                              OrderStatus = x.OrderStatus,
                                                              OrderCategory = x.Category,
                                                              Region = x.Region,
                                                              Territory = x.Territory,
                                                              Language = x.Language,
                                                              RequestNumber = x.RequestNumber,
                                                              MPO = x.MPO,
                                                              HALID = x.HALID,
                                                              VendorId = x.VendorId,
                                                              FirstAnnouncementDate = x.FirstAnnouncedDate != null ? x.FirstAnnouncedDate.Value.ToString("MM/dd/yyyy") : null,
                                                              DueDate = x.DeliveryDueDate != null ? x.DeliveryDueDate.Value.ToString("MM/dd/yyyy") : null,
                                                              GreenlightSent = x.GreenlightSenttoPackaging != null ? x.GreenlightSenttoPackaging.Value.ToString("MM/dd/yyyy") : null,
                                                              GreenlightReceived = x.GreenlightValidatedbyDMDM != null ? x.GreenlightValidatedbyDMDM.Value.ToString("MM/dd/yyyy") : null,
                                                              DeliveryDate= x.ActualDeliveryDate != null ? x.ActualDeliveryDate.Value.ToString("MM/dd/yyyy") : null,
                                                              AssetRequired = x.LanguageType,
                                                              ESTUPC = x.ESTUPC,
                                                              VODUPC = x.IVODUPC,
                                                              LockedBy=x.LockedBy,
                                                              LockedOn=x.LockedOn,
                                                              IsLocked=x.IsLocked
                                                              //OrderType=x.OrderType

                                                          }).Distinct().OrderBy(x => x.OrderId).ToList();

            return orders;
        }

        public List<Order> SearchOrders(string whrSQLquery, string channel)
        {
            var _context = new DeluxeOrderManagementEntities();
            var orders = _context.usp_Get_Orders(whrSQLquery, channel).Select(x =>
                                                       new Order
                                                       {
                                                           AnnouncemntId = x.AnnouncementId,
                                                           OrderId = x.OrderID,
                                                           //VId = x.VId,
                                                           Title = x.Title,
                                                           OrderStatus = x.OrderStatus,
                                                           OrderCategory = x.Category,
                                                           Region = x.Region,
                                                           Territory = x.Territory,
                                                           Language = x.Language,
                                                           RequestNumber = x.RequestNumber,
                                                           MPO = x.MPO,
                                                           HALID = x.HALID,
                                                           VendorId = x.VendorId,
                                                           FirstAnnouncementDate = x.FirstAnnouncedDate != null ? x.FirstAnnouncedDate.Value.ToString("MM/dd/yyyy") : null,
                                                           DueDate = x.DeliveryDueDate != null ? x.DeliveryDueDate.Value.ToString("MM/dd/yyyy") : null,
                                                           GreenlightSent = x.GreenlightSenttoPackaging != null ? x.GreenlightSenttoPackaging.Value.ToString("MM/dd/yyyy") : null,
                                                           GreenlightReceived = x.GreenlightValidatedbyDMDM != null ? x.GreenlightValidatedbyDMDM.Value.ToString("MM/dd/yyyy") : null,
                                                           DeliveryDate = x.ActualDeliveryDate != null ? x.ActualDeliveryDate.Value.ToString("MM/dd/yyyy") : null,
                                                           AssetRequired = x.LanguageType,
                                                           ESTUPC = x.ESTUPC,
                                                           VODUPC = x.IVODUPC,
                                                           LockedBy = x.LockedBy,
                                                           LockedOn = x.LockedOn,
                                                           IsLocked = x.IsLocked
                                                           //OrderType=x.OrderType

                                                       }).Distinct().ToList();

            return orders;
        }

        public Order SearchEditOrder(string whrSQLquery)
        {
            var _context = new DeluxeOrderManagementEntities();
            var order = _context.usp_Get_Orders(whrSQLquery, string.Empty).Select(x =>
                                                        new Order
                                                        {
                                                            AnnouncemntId = x.AnnouncementId,
                                                            OrderId = x.OrderID,
                                                            //VId = x.VId,
                                                            Title = x.Title,
                                                            OrderStatus = x.OrderStatus,
                                                            OrderCategory = x.Category,
                                                            Region = x.Region,
                                                            Territory = x.Territory,
                                                            Language = x.Language,
                                                            RequestNumber = x.RequestNumber,
                                                            MPO = x.MPO,
                                                            HALID = x.HALID,
                                                            VendorId = x.VendorId,
                                                            FirstAnnouncementDate = x.FirstAnnouncedDate != null ? x.FirstAnnouncedDate.Value.ToString("MM/dd/yyyy") : null,
                                                            DueDate = x.DeliveryDueDate != null ? x.DeliveryDueDate.Value.ToString("MM/dd/yyyy") : null,
                                                            GreenlightSent = x.GreenlightSenttoPackaging != null ? x.GreenlightSenttoPackaging.Value.ToString("MM/dd/yyyy") : null,
                                                            GreenlightReceived = x.GreenlightValidatedbyDMDM != null ? x.GreenlightValidatedbyDMDM.Value.ToString("MM/dd/yyyy") : null,
                                                            DeliveryDate = x.ActualDeliveryDate != null ? x.ActualDeliveryDate.Value.ToString("MM/dd/yyyy") : null,
                                                            AssetRequired = x.LanguageType,
                                                            ESTUPC = x.ESTUPC,
                                                            VODUPC = x.IVODUPC,
                                                            LockedBy = x.LockedBy,
                                                            LockedOn = x.LockedOn,
                                                            IsLocked = x.IsLocked
                                                            //OrderType=x.OrderType

                                                        }).FirstOrDefault();

            return order;
        }

        public bool EditOrder(Order order)
        {
            bool status = false;
            var _context = new DeluxeOrderManagementEntities();
            VID vid = new VID();

            var announcement = (from ann in _context.AnnouncementGrids
                                where ann.Id==order.AnnouncemntId
                                select ann).FirstOrDefault();
            var vidEty = (from vids in _context.VIDs
                          where vids.VendorId == order.VendorId && vids.VideoVersion == announcement.VideoVersion
                          select vids).FirstOrDefault();
            if (vidEty == null)
            {
                vid.VIDStatus = "PRIMARY";
                vid.VendorId = order.VendorId;
                vid.VideoVersion = announcement.VideoVersion;
                vid.TitleName = announcement.Title;
                vid.TitleCategory = order.OrderCategory;
                vid.EditName = announcement.LocalEdit;
                vid.CreatedOn = DateTime.UtcNow;
                vid.ModifiedOn = DateTime.UtcNow;
                vid.CreatedBy = "Deluxe";
                vid.ModifiedBy = "Deluxe";
                _context.VIDs.Add(vid);
                _context.SaveChanges();
                status = true;
            }
        


            //var vidEty = _context.VIDs.Where(x => x.Id == order.VId).FirstOrDefault();
            //var orders = _context.OrderGrids.Where(x => x.VIDId == order.VId).ToList();
            //var orderExceptCurrent = _context.OrderGrids.Where(x => x.VIDId == order.VId && x.Id != order.OrderId).ToList();
            //if (orders.Count == 1 /*|| orderExceptCurrent.Count==1*/)
            //{
            //    vidEty.VendorId = order.VendorId;
            //    status = true;
            //}
            //else
            //{
            //    var announcementEty = _context.AnnouncementGrids.Where(x => x.Id == order.AnnouncemntId).FirstOrDefault();
            //    vid.VendorId = order.VendorId;
            //    vid.VideoVersion = announcementEty.VideoVersion;
            //    vid.TitleName = announcementEty.Title;
            //    vid.TitleCategory = order.OrderCategory;
            //    vid.EditName = announcementEty.LocalEdit;
            //    vid.CreatedOn = DateTime.UtcNow;
            //    vid.ModifiedOn = DateTime.UtcNow;
            //    vid.CreatedBy = "Deluxe";
            //    vid.ModifiedBy = "Deluxe";
            //    _context.VIDs.Add(vid);
            //    _context.SaveChanges();
            //    status = true;
            //}
            var ety = _context.OrderGrids.Where(x => x.Id == order.OrderId).FirstOrDefault();
            if (ety != null)
            {
                ety.RequestNumber = order.RequestNumber;
                ety.HALID = order.HALID;
                ety.MPO = order.MPO;
                ety.DeliveryDueDate = ParseDate(order.DueDate);
                ety.GreenlightSenttoPackaging = ParseDate(order.GreenlightSent);
                ety.GreenlightValidatedbyDMDM = ParseDate(order.GreenlightReceived);
                ety.ActualDeliveryDate = ParseDate(order.DeliveryDate);
                ety.ESTUPC = order.ESTUPC;
                ety.IVODUPC = order.VODUPC;
                ety.Category = order.OrderCategory;
                ety.OrderStatus = order.OrderStatus;
                ety.VendorId = order.VendorId;
                //ety.VIDId = (orders.Count == 1 /*|| orderExceptCurrent.Count==1*/) ? vidEty.Id : vid.Id;
                status = true;

            }
            if (status)
            {
                _context.SaveChanges();
                _context.usp_UpdateOG_Status(order.OrderId);
            }
            return status;
        }

        public OrderSearch GetSearchValue()
        {
            OrderSearch orderSearch = new OrderSearch();
            var _context = new DeluxeOrderManagementEntities();
            var selectedTitles = (from ann in _context.AnnouncementGrids
                                  select ann.Title.ToString()).Distinct().OrderBy(x=>x).ToList();
            var titleCategory = (from og in _context.OrderGrids
                                 select og.Category.ToString()).Distinct().OrderBy(x => x).ToList();
            var contentProviders = (from customer in _context.Customers
                                    where customer.Type == (int)Customers.ContentProvider
                                    select customer.Name.ToString()
                                  ).Distinct().ToList();
            var contentDistributors = (from customer in _context.Customers
                                       where customer.Type == (int)Customers.ContentDistributor
                                       select customer.Name.ToString()
                                  ).Distinct().ToList();






            //var localEdit = (from ann in _context.AnnouncementGrids
            //                 select new ChkValues
            //                 {
            //                     Name = ann.LocalEdit,
            //                     IsSelected = false
            //                 }).Distinct().ToList();
            //var orderStatus = (from order in _context.OrderGrids
            //                   where order.OrderStatus != "N/A" && order.OrderStatus != null
            //                   select new ChkValues
            //                   {
            //                       Name = order.OrderStatus,
            //                       IsSelected = false
            //                   }).Distinct().ToList();
            //var salesChannel= (from order in _context.SalesChannels
            //                   select new ChkValues
            //                   {
            //                       Name = order.Name,
            //                       IsSelected = false
            //                   }).Distinct().ToList();






            var orderStatus = (from order in _context.OrderGrids
                               where order.OrderStatus != "N/A" && order.OrderStatus != null
                               select order.OrderStatus.ToString()).Distinct().OrderBy(x => x).ToList();
            var localEdits = (from ann in _context.AnnouncementGrids
                              select ann.LocalEdit.ToString()).Distinct().ToList();
            var salesChannels = (from saleChannel in _context.SalesChannels
                                 select saleChannel.Name.ToString()).Distinct().ToList();
            var territories = (from trr in _context.Territories
                               select trr.WBTerritory.ToString()).Distinct().OrderBy(x => x).ToList();

            orderSearch.SelectedTitles = selectedTitles;
            orderSearch.EditTypes = titleCategory;
            orderSearch.ContentProviders = contentProviders;
            orderSearch.ContentDistributors = contentDistributors;
            orderSearch.OrderStatuses = orderStatus;
            orderSearch.LocalEdits = localEdits;
            orderSearch.MediaTypes = salesChannels;
            orderSearch.Territories = territories;
            return orderSearch;
        }
        public TerritoryLanguage GetDropDownValue()
        {
            var _context = new DeluxeOrderManagementEntities();
            TerritoryLanguage territoryLanguage = new TerritoryLanguage();
            var orderCategory = (from og in _context.OrderGrids
                                 where !string.IsNullOrEmpty(og.Category)
                                 select og.Category.ToString()
                                 ).Distinct().ToList();
            var territory = _context.Territories.Select(x => new DeluxeOM.Models.Territory()
            {
                TerritoryID = x.Id,
                TerritoryName = x.WBTerritory
            }).Distinct().OrderBy(x => x.TerritoryName).ToList();
            var language = _context.Languages.Select(x => new DeluxeOM.Models.Language()
            {
                LanguageID = x.Id,
                LanguageName = x.Name
            }).Distinct().OrderBy(x => x.LanguageName).ToList();

            var title = _context.AnnouncementGrids.Select(x => new DeluxeOM.Models.Title()
            {
                //TitleID = x.Id,
                Name = x.Title
            }).Distinct().OrderBy(x=>x.Name).ToList();

            territoryLanguage.Territory = territory;
            territoryLanguage.Language = language;
            territoryLanguage.Title = title;
            territoryLanguage.orderCategory = orderCategory;
            return territoryLanguage;
        }

        private DateTime? ParseDate(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return null;
            }
            else
            {
                return DateTime.ParseExact(date, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public OrderSaveStatus CreateOrder(Order order, List<string> assets)
        {
            OrderSaveStatus orderStatus = new OrderSaveStatus();
            var _context = new DeluxeOrderManagementEntities();
            int languageId = Convert.ToInt32(order.Language);
            int territoryId = Convert.ToInt32(order.Territory);
            var announcement = (from ann in _context.AnnouncementGrids
                                where ann.Title == order.Title &&
                                      ann.LanguageId == languageId &&
                                      ann.TerritoryId == territoryId
                                select ann).FirstOrDefault();

            if (announcement != null)
            {
                var orders = (from og in _context.OrderGrids
                              where og.AnnouncementId == announcement.Id && assets.Contains(og.LanguageType)
                              select og).ToList();
                var vids = (from v in _context.VIDs
                            where v.VendorId == order.VendorId && v.VideoVersion == announcement.VideoVersion
                            select v
                          ).ToList();
                VID vid = null;
                if (orders.Count == 0 || orders == null)
                {
                    if (vids.Count == 0 || vids == null)
                    {
                        vid = new VID();
                        vid.VIDStatus = "PRIMARY";
                        vid.VendorId = order.VendorId;
                        vid.VideoVersion = announcement.VideoVersion;
                        vid.TitleName = announcement.Title;
                        vid.TitleCategory = order.OrderCategory;
                        vid.EditName = announcement.LocalEdit;
                        vid.CreatedOn = DateTime.UtcNow;
                        vid.ModifiedOn = DateTime.UtcNow;
                        vid.CreatedBy = "Deluxe";
                        vid.ModifiedBy = "Deluxe";
                        _context.VIDs.Add(vid);
                        _context.SaveChanges();
                    }
                    foreach (var asset in assets)
                    {
                        OrderGrid orderGrid = new OrderGrid()
                        {
                            AnnouncementId = announcement.Id,
                            OrderStatus = order.OrderStatus,
                            MPO = order.MPO,
                            HALID = order.HALID,
                            Category = order.OrderCategory,
                            RequestNumber = order.RequestNumber,
                            DeliveryDueDate = ParseDate(order.DueDate),
                            GreenlightSenttoPackaging = ParseDate(order.GreenlightSent),
                            GreenlightValidatedbyDMDM = ParseDate(order.GreenlightReceived),
                            ActualDeliveryDate = ParseDate(order.DeliveryDate),
                            ESTUPC = order.ESTUPC,
                            IVODUPC = order.VODUPC,
                            LanguageType = asset,
                            CustomerId = 1,
                            VendorId = (vid != null) ? vid.VendorId : vids.FirstOrDefault().VendorId

                        };
                        _context.OrderGrids.Add(orderGrid);
                        _context.SaveChanges();
                        _context.usp_UpdateOG_Status(orderGrid.Id);
                    }
                    orderStatus.ErrorMessage = Constant.orderSaved;
                    orderStatus.IsSaved = true;
                }
                else
                {
                    orderStatus.ErrorMessage = Constant.orderExist;
                    orderStatus.IsSaved = false;
                }
            }
            else
            {
                orderStatus.ErrorMessage = Constant.announcementNotExist;
                orderStatus.IsSaved = false;
            }
            return orderStatus;
        }

        public void LogError(ErrorLog error)
        {

            var _context = new DeluxeOrderManagementEntities();
            _context.ErrorLogs.Add(error);
            _context.SaveChanges();
        }

        public PipelineOrder GetPipeLineOrder(string requestNumber)
        {
            var _context = new DeluxeOrderManagementEntities();
            var pipeLineOrder = (from po in _context.PipelineOrders
                                 where po.CableLabsAssetID == requestNumber
                                 select po
                               ).FirstOrDefault();
            return pipeLineOrder;
        }

        public void LockOrder(int orderId, int userId)
        {
            var _context = new DeluxeOrderManagementEntities();
            var order = _context.OrderGrids.Where(x => x.Id == orderId).FirstOrDefault();
            order.IsLocked = true;
            order.LockedBy = userId;
            order.LockedOn = DateTime.UtcNow;
            _context.SaveChanges();
        }

        public void UnlockOrder(int? orderId, int userId)
        {
            var _context = new DeluxeOrderManagementEntities();
            if (orderId != null)
            {
                var order = _context.OrderGrids.Where(x => x.Id == orderId && x.LockedBy == userId).FirstOrDefault();
                if (order != null)
                {
                    order.IsLocked = false;
                    order.LockedBy = null;
                    order.LockedOn = null;
                }
            }
            else
            {
                var order = _context.OrderGrids.Where(x => x.LockedBy == userId).ToList();
                foreach (var o in order)
                {
                    o.IsLocked = false;
                    o.LockedBy = null;
                    o.LockedOn = null;
                }
            }

            _context.SaveChanges();
        }
    }
}
