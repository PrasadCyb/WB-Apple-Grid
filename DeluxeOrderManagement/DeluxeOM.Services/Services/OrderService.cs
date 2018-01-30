//using DeluxeOM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeluxeOM.Models;
using DeluxeOM.Utils.Config;

//using DeluxeOM.Repository.Interfaces;
//using DeluxeOM.Repository.Implementation;
using DeluxeOM.Repository;

namespace DeluxeOM.Services
{
    public class OrderService : IOrderService
    {
        IOrderRepository _repository = null;
        string notApplicable = "N/A";
        string blanks = "''";
        public OrderService()
        {
            _repository = new OrderRepository();
        }
        public List<Order> GetOrders()
        {
            string whrSQLquery = string.Empty;
            List<Order> orders = _repository.GetOrders(whrSQLquery);
            return orders;
        }

        public List<Order> SearchOrders(OrderSearch orderSearch)
        {
            string whrSQLquery = string.Empty;
            string channel = string.Empty;
            string temp = string.Empty;
            if (!string.IsNullOrEmpty(orderSearch.StartDate) && !string.IsNullOrEmpty(orderSearch.EndDate))
            {
                //orderSearch.ESTStartDate = Convert.ToDateTime(orderSearch.StartDate);
                //orderSearch.ESTEndDate = Convert.ToDateTime(orderSearch.EndDate);
                orderSearch.ESTStartDate = DateTime.ParseExact(orderSearch.StartDate, "MM/dd/yyyy", null);
                orderSearch.ESTEndDate = DateTime.ParseExact(orderSearch.EndDate, "MM/dd/yyyy", null);
            }
            if (!string.IsNullOrEmpty(orderSearch.SelectedTitle))
            {
                temp = orderSearch.SelectedTitle;
                orderSearch.SelectedTitle = orderSearch.SelectedTitle.Replace("'", "\''");
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " AG.Title ='" + orderSearch.SelectedTitle + "'  " : whrSQLquery + " AND AG.Title = '" + orderSearch.SelectedTitle + "' ";
            }

            if (!string.IsNullOrEmpty(orderSearch.EditType))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " O.Category ='" + orderSearch.EditType + "'  " : whrSQLquery + " AND O.Category = '" + orderSearch.EditType + "' ";
            }

            if (!string.IsNullOrEmpty(orderSearch.LocalEdit))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " AG.LocalEdit ='" + orderSearch.LocalEdit + "'  " : whrSQLquery + " AND AG.LocalEdit = '" + orderSearch.LocalEdit + "' ";
            }
            if (!string.IsNullOrEmpty(orderSearch.OrderStatus))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " O.OrderStatus ='" + orderSearch.OrderStatus + "'  " : whrSQLquery + " AND O.OrderStatus = '" + orderSearch.OrderStatus + "' ";
            }
            if (!string.IsNullOrEmpty(orderSearch.GreenLightSent))
            {
                if (orderSearch.GreenLightSent.Equals("Yes"))
                {
                    whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " O.GreenlightSenttoPackaging IS NOT NULL " : whrSQLquery + " AND O.GreenlightSenttoPackaging IS NOT NULL ";
                    //whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " O.GreenlightSenttoPackaging IS NOT NULL OR O.GreenlightSenttoPackaging <>'" + blanks + "'" : whrSQLquery + "AND O.GreenlightSenttoPackaging IS NOT NULL OR O.GreenlightSenttoPackaging <>'" + blanks + "'";
                }
                else if (orderSearch.GreenLightSent.Equals("No"))
                {
                    whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " O.GreenlightSenttoPackaging IS NULL " : whrSQLquery + " AND O.GreenlightSenttoPackaging IS NULL ";
                    //whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " O.GreenlightSenttoPackaging IS NULL OR O.GreenlightSenttoPackaging ='" + blanks + "'" : whrSQLquery + "AND O.GreenlightSenttoPackaging IS NULL OR O.GreenlightSenttoPackaging ='" + blanks + "'";
                }
            }
            if (!string.IsNullOrEmpty(orderSearch.GreenLightReceived))
            {
                if (orderSearch.GreenLightReceived.Equals("Yes"))
                {
                    whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " (O.GreenlightValidatedbyDMDM IS NOT NULL ) " : whrSQLquery + " AND (O.GreenlightValidatedbyDMDM IS NOT NULL ) ";
                }
                else if (orderSearch.GreenLightReceived.Equals("No"))
                {
                    whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " (O.GreenlightValidatedbyDMDM IS NULL ) " : whrSQLquery + " AND (O.GreenlightValidatedbyDMDM IS NULL ) ";
                }
            }
            if (!string.IsNullOrEmpty(orderSearch.Region))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " R.NAME ='" + orderSearch.Region + "'  " : whrSQLquery + " AND R.NAME = '" + orderSearch.Region + "' ";
            }
            if (!string.IsNullOrEmpty(orderSearch.Territory))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " T.WBTerritory ='" + orderSearch.Territory + "'  " : whrSQLquery + " AND T.WBTerritory = '" + orderSearch.Territory + "' ";
            }

            if ((orderSearch.ESTStartDate != null && !(orderSearch.ESTStartDate.Equals(DateTime.MinValue))) && (orderSearch.ESTEndDate != null && !(orderSearch.ESTEndDate.Equals(DateTime.MinValue))))
            {
                string startDate = orderSearch.ESTStartDate.Date.ToString("yyyy-MM-dd");
                string endDate = orderSearch.ESTEndDate.AddDays(1).Date.ToString("yyyy-MM-dd");
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " (CF.MinClientStartDate >='" + startDate + "' AND CF.MaxClientEndDate <'" + endDate + "')  " : whrSQLquery + " AND (CF.MinClientStartDate >='" + startDate + "' AND CF.MaxClientEndDate <'" + endDate + "') ";
            }
            //if (orderSearch.Territory != null && orderSearch.Territory.Count() != 0)
            //{
            //    string territories = null;
            //    foreach (var x in orderSearch.Territory)
            //    {
            //        territories += "'" + x + "'" + ",";
            //    }
            //    territories = territories.Substring(0, territories.Length - 1);
            //    whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " T.WBTerritory IN(" + territories + ")  " : whrSQLquery + " AND T.WBTerritory IN(" + territories + ") ";
            //}
            if (!string.IsNullOrEmpty(orderSearch.MediaType))
            {
                channel = string.IsNullOrEmpty(channel) ? " C.Channel ='" + orderSearch.MediaType + "'  " : whrSQLquery + " AND C.Channel = '" + orderSearch.MediaType + "' ";
            }
            else
            {
                string alllChannel = "'EST'" + "," + "'POEST'" + "," + "'VOD'";
                channel = string.IsNullOrEmpty(channel) ? " C.Channel IN(" + alllChannel + ")  " : whrSQLquery + " AND C.Channel IN(" + alllChannel + ") ";
            }
            if (!string.IsNullOrEmpty(whrSQLquery))
            {
                whrSQLquery = "where" + whrSQLquery;
            }

            if (!string.IsNullOrEmpty(channel))
            {
                channel = "where" + channel;
            }

            if (!string.IsNullOrEmpty(orderSearch.SortBy))
            {
                whrSQLquery = whrSQLquery + " ORDER BY " + " " + orderSearch.SortBy + " " + orderSearch.SortOrder;
            }

            // this if block is to resore title name which is modified for search title with '
            if (!string.IsNullOrEmpty(temp))
            {
                orderSearch.SelectedTitle = temp;
            }


            List<Order> orders = _repository.SearchOrders(whrSQLquery, channel);
            return orders;
        }

        public Order SearchEditOrder(int id, int annId)
        {
            string whrSQLquery = string.Empty;
            whrSQLquery = " O.Id='" + id + "' AND AG.Id='" + annId + "'";
            whrSQLquery = "where" + whrSQLquery;
            var order = _repository.SearchEditOrder(whrSQLquery);
            return order;
        }

        public bool EditOrder(Order SaveOrder)
        {
            var order = UpdateOrder(SaveOrder);
            bool status = _repository.EditOrder(order);
            return true;
        }
        public OrderSearch GetSearchValues()
        {
            OrderSearch orderSearch = _repository.GetSearchValue();
            //var chkValues = new List<ChkValues>(){new ChkValues()
            //                                        {   Name="Yes",
            //                                            IsSelected=false
            //                                        },
            //                                      new ChkValues()
            //                                        {   Name="No",
            //                                            IsSelected=false
            //                                        }
            //                                      };

            var greenLights = new List<string>() { "Yes", "No" };

            orderSearch.SortByList = new List<SortSearch>()
            {
                new SortSearch {Text="Order Status", Value="O.OrderStatus" },
                new SortSearch {Text="Order Category", Value="O.Category" },
                new SortSearch {Text="Territory", Value="Territory" },
                new SortSearch {Text="Language", Value="Language" },
                new SortSearch {Text="Request No", Value="RequestNumber" },
                new SortSearch {Text="PO No", Value="MPO" },
                new SortSearch {Text="HAL ID", Value="HALID" },
                new SortSearch {Text="Vendor ID", Value="VendorId" },
                new SortSearch {Text="First Start Date", Value="FirstAnnouncedDate" },
                new SortSearch {Text="Due Date", Value="DeliveryDueDate" },
                new SortSearch {Text="Greenlight Sent", Value="GreenlightSenttoPackaging" },
                new SortSearch {Text="Greenlight Received", Value="GreenlightValidatedbyDMDM" },
                new SortSearch {Text="Asset Required", Value="LanguageType" },
                new SortSearch {Text="EST UPC", Value="ESTUPC" },
                new SortSearch {Text="VOD UPC", Value="IVODUPC" },
                new SortSearch {Text="Delivery Date", Value="ActualDeliveryDate" }
            };
            orderSearch.SortOrderList = new List<SortSearch>()
            {
                new SortSearch {Text="Ascending", Value="asc" },
                new SortSearch {Text="Descending", Value="desc" }
            };
            orderSearch.GreenLights = greenLights;
            orderSearch.GreenLights = greenLights;
            return orderSearch;
        }

        public TerritoryLanguage GetDropDownValue()
        {
            var chkValues = new List<ChkValues>(){new ChkValues()
                                                    {   Name="Audio",
                                                        IsSelected=false
                                                    },
                                                  new ChkValues()
                                                    {   Name="Audio Description",
                                                        IsSelected=false
                                                    },
                                                  new ChkValues()
                                                    {   Name="Caption",
                                                        IsSelected=false
                                                    },
                                                  new ChkValues()
                                                    {   Name="Video",
                                                        IsSelected=false
                                                    },
                                                  new ChkValues()
                                                    {   Name="Forced Subtitle",
                                                        IsSelected=false
                                                    },
                                                  new ChkValues()
                                                    {   Name="Preview Films",
                                                        IsSelected=false
                                                    },
                                                  new ChkValues()
                                                    {   Name="Sub",
                                                        IsSelected=false
                                                    }
                                                  };
            TerritoryLanguage territoryLanguage = _repository.GetDropDownValue();
            territoryLanguage.Assets = chkValues;
            return territoryLanguage;
        }

        public OrderSaveStatus CreateOrder(Order saveOrder)
        {
            var order = UpdateOrder(saveOrder);
            List<string> assets = order.Assets.Where(x => x.IsSelected == true).Select(x => x.Name).ToList();
            var orderStatus = _repository.CreateOrder(order, assets);
            return orderStatus;
        }

        public void LogError(ErrorMgt error)
        {
            ErrorLog errorlog = new ErrorLog { ErrorName = error.ErrorMessage };
            _repository.LogError(errorlog);
        }

        public Order UpdateOrder(Order order)
        {
            if (!string.IsNullOrEmpty(order.RequestNumber))
            {
                var pipeLineOrder = _repository.GetPipeLineOrder(order.RequestNumber);
                if (pipeLineOrder != null)
                {
                    order.MPO = pipeLineOrder.PurchaseOrder;
                    order.HALID = pipeLineOrder.HALOrderID;
                }
                
            }
            else
            {
                order.MPO = string.Empty;
                order.HALID = string.Empty;
            }
            if (string.IsNullOrEmpty(order.OrderStatus))
            {
                if (!string.IsNullOrEmpty(order.RequestNumber) && (!string.IsNullOrEmpty(order.MPO) || !string.IsNullOrEmpty(order.HALID)))
                {
                    order.OrderStatus = OrderStaus.Processing.ToString();
                }
                else if (!string.IsNullOrEmpty(order.RequestNumber))
                {
                    order.OrderStatus = "Request Received";
                }
                else
                {
                    order.OrderStatus = OrderStaus.New.ToString();
                }
            }
            if (!order.OrderStatus.Equals(OrderStaus.Cancelled.ToString()) && !order.OrderStatus.Equals(OrderStaus.Complete.ToString()))
            {
                if (!string.IsNullOrEmpty(order.RequestNumber) && (!string.IsNullOrEmpty(order.MPO) || !string.IsNullOrEmpty(order.HALID)))
                {
                    order.OrderStatus = OrderStaus.Processing.ToString();
                }
                else if (!string.IsNullOrEmpty(order.RequestNumber))
                {
                    order.OrderStatus = "Request Received";
                }
                else
                {
                    order.OrderStatus = OrderStaus.New.ToString();
                }
            }
            return order;
        }

        public void LockOrder(int orderId, int userId)
        {
            _repository.LockOrder(orderId,userId);
        }

        public void UnlockOrder(int? orderId, int userId)
        {
            _repository.UnlockOrder(orderId, userId);
        }
    }
}
