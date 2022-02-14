using System;
using System.Collections.Generic;
using System.Text;

namespace com.aadviktech.IMS.ViewModel
{
    public enum SortingOrder
    {
        Ascending = 1,
        Descending = 2
    }

    public class DataPagingModel
    {
        public DataPagingModel()
        {
            CurrentPageID = 0;
            PageSize = 10;
            TotalRecords = 0;
            TotalPages = 0;
            PageUrl = "";
            SearchText = "";
            SortingColumn = "";
            Paging = true;
            SortingOrder = SortingOrder.Ascending;
            DistributorId = 0;
            Requester = 0;


        }

      
        public int CurrentUserId { get; set; }
        public bool Paging { get; set; }
        public int CurrentPageID { get; set; }
        public int TotalPages { get; set; }
        public int StartPageNumber { get; set; }
        public int EndPageNumber { get; set; }

        public int TotalRecords { get; set; }
        public int StartRecord { get; set; }
        public int EndRecord { get; set; }

        public int PageSize { get; set; }
        public string PageUrl { get; set; }
        public string AjaxUrl { get; set; }

        public string SortingColumn { get; set; }
        public SortingOrder SortingOrder { get; set; }
        public int DistributorId { get; set; }
        public string SearchText { get; set; }
        public int Requester { get; set; }

        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }

        private Dictionary<string, string> _SearchParameter = new Dictionary<string, string>();
        public Dictionary<string, string> SearchParameter { get { return _SearchParameter; } set { _SearchParameter = value; } }
    }

    public static class ExtensionMethod
    {
        public static int TableSkipRecord(this int value, int? CurrentPageSize)
        {
            if (CurrentPageSize.HasValue)
            {
                value = value == 0 ? 0 : (value - 1) * (CurrentPageSize.Value);
            }
            return value;
        }
    }

}
