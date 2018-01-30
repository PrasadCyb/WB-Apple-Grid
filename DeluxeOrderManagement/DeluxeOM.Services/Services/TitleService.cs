using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeluxeOM.Models;
using DeluxeOM.Repository;

namespace DeluxeOM.Services
{
    public class TitleService : ITitleService
    {

        ITitleRepository _repository = null;
        public TitleService()
        {
            _repository = new TitleRepository();
        }

        public TitleSearch GetSearchValues()
        {
            var searchValues = _repository.GetSearchValues();
            searchValues.ComponentTypes = new List<string>() { "Audio", "Video","Sub" };
            searchValues.SortByList = new List<SortSearch>()
            {
                new SortSearch {Text="Title Name", Value="TitleName" },
                new SortSearch {Text="Vendor ID", Value="VendorId" },
                new SortSearch {Text="VID Status", Value="VIDStatus" },
                new SortSearch {Text="Territory", Value="AppleTerritory" },
                new SortSearch {Text="Language", Value="LanguageName" },
                new SortSearch {Text="Pre Order Start Date", Value="POESTStartDate" },
                new SortSearch {Text="EST Start Date", Value="ESTStartDate" },
                new SortSearch {Text="EST End Date", Value="ESTEndDate" },
                new SortSearch {Text="ESTHD Live Status", Value="LiveESTHD" },
                new SortSearch {Text="ESTSD Live Status", Value="LiveESTSD" },
                new SortSearch {Text="VOD Start Date", Value="VODStartDate" },
                new SortSearch {Text="VOD End Date", Value="VODEndDate" },
                new SortSearch {Text="VODHD Live Status", Value="LiveVODHD" },
                new SortSearch {Text="VODSD Live Status", Value="LiveVODSD" },
                new SortSearch {Text="Component Type", Value="ComponentType" },
                new SortSearch {Text="Component Quality", Value="ComponentQuality" },
                new SortSearch {Text="Language Type", Value="LanguageType" },
                new SortSearch {Text="Language Status", Value="LanguageStatus" },
            };
            searchValues.SortOrderList = new List<SortSearch>()
            {
                new SortSearch {Text="Ascending", Value="asc" },
                new SortSearch {Text="Descending", Value="desc" }
            };
            return searchValues;
        }

        public List<TitleList> GetTitles()
        {
            string whrSQLquery = string.Empty;
            var titles=_repository.GetTitles(whrSQLquery);
            return titles;
        }

        public List<TitleList> SearchTitles(TitleSearch titleSearch)
        {
            string whrSQLquery = string.Empty;
            string temp = string.Empty;
            if (!string.IsNullOrEmpty(titleSearch.StartDate) && !string.IsNullOrEmpty(titleSearch.EndDate))
            {
                //titleSearch.ESTStartDate = Convert.ToDateTime(titleSearch.StartDate);
                //titleSearch.ESTEndDate = Convert.ToDateTime(titleSearch.EndDate);
                titleSearch.ESTStartDate = DateTime.ParseExact(titleSearch.StartDate, "MM/dd/yyyy", null);
                titleSearch.ESTEndDate = DateTime.ParseExact(titleSearch.EndDate, "MM/dd/yyyy", null);
            }
            if (!string.IsNullOrEmpty(titleSearch.SelectedTitle))
            {
                temp = titleSearch.SelectedTitle;
                titleSearch.SelectedTitle = titleSearch.SelectedTitle.Replace("'", "\''");
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " TitleName ='" + titleSearch.SelectedTitle + "'  " : whrSQLquery + " AND TitleName = '" + titleSearch.SelectedTitle + "' ";
            }
            if (!string.IsNullOrEmpty(titleSearch.EditType))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " Category ='" + titleSearch.EditType + "'  " : whrSQLquery + " AND Category = '" + titleSearch.EditType + "' ";
            }
            if (!string.IsNullOrEmpty(titleSearch.Territory))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " AppleTerritory ='" + titleSearch.Territory + "'  " : whrSQLquery + " AND AppleTerritory = '" + titleSearch.Territory + "' ";
            }

            if (!string.IsNullOrEmpty(titleSearch.Language))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " LanguageName ='" + titleSearch.Language + "'  " : whrSQLquery + " AND LanguageName = '" + titleSearch.Language + "' ";
            }

            if (!string.IsNullOrEmpty(titleSearch.VideoVersion))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " VideoVersion ='" + titleSearch.VideoVersion + "'  " : whrSQLquery + " AND VideoVersion = '" + titleSearch.VideoVersion + "' ";
            }

            if ((titleSearch.ESTStartDate != null && !(titleSearch.ESTStartDate.Equals(DateTime.MinValue))) && (titleSearch.ESTEndDate != null && !(titleSearch.ESTEndDate.Equals(DateTime.MinValue))))
            {
                string estStartDate = titleSearch.ESTStartDate.Date.ToString("yyyy-MM-dd");
                string estEndDate = titleSearch.ESTEndDate.AddDays(1).Date.ToString("yyyy-MM-dd");
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " (ESTStartDate >='" + estStartDate + "' AND ESTEndDate <'" + estEndDate + "')  " : whrSQLquery + " AND (ESTStartDate >='" + estStartDate + "' AND ESTEndDate <'" + estEndDate + "') ";
            }

            if (!string.IsNullOrEmpty(titleSearch.MPM))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " MPM ='" + titleSearch.MPM + "'  " : whrSQLquery + " AND MPM = '" + titleSearch.MPM + "' ";
            }

            if (!string.IsNullOrEmpty(titleSearch.VendorId))
            {
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " VendorId ='" + titleSearch.VendorId + "'  " : whrSQLquery + " AND VendorId = '" + titleSearch.VendorId + "' ";
            }

            if (!string.IsNullOrEmpty(titleSearch.ComponentType))
            {
                string componentTypes = string.Empty;
                if (titleSearch.ComponentType.Equals("Audio"))
                {
                    componentTypes = "'AUDIO'" + "," + "'FORCED_SUBTITLES'" + "," + "'DUB_CREDIT'" + "," + "'AUDIO_DESCRIPTION'";
                }
                else if (titleSearch.ComponentType.Equals("Sub"))
                {
                    componentTypes = "'SUBTITLES'" + "," + "'CAPTIONS'";
                }
                else if (titleSearch.ComponentType.Equals("Video"))
                {
                    componentTypes = "'VIDEO'";
                }
                whrSQLquery = string.IsNullOrEmpty(whrSQLquery) ? " ComponentType IN(" + componentTypes + ")" : whrSQLquery + " AND ComponentType IN(" + componentTypes + ") ";
            }

            
            if (!string.IsNullOrEmpty(whrSQLquery))
            {
                whrSQLquery = "where" + whrSQLquery;
            }

            if (!string.IsNullOrEmpty(titleSearch.SortBy))
            {
                whrSQLquery = whrSQLquery + " ORDER BY "+" "+ titleSearch.SortBy + " " + titleSearch.SortOrder;
            }
            // this if block is to resore title name which is modified for search title with '
            if (!string.IsNullOrEmpty(temp))
            {
                titleSearch.SelectedTitle = temp;
            }
            var titles = _repository.SearchTitles(whrSQLquery);
            return titles;
        }
    }
}
