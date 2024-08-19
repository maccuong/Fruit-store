using Clothing_boutique_web.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Clothing_boutique_web.Areas.Admin.Models
{
    public class PaginatedList<T> : List<T>
    {
        public IEnumerable<T> ListItems { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public PaginatedList(List<T> _ListItems, PagingInfo _PagingInfo)
        {
            ListItems  = _ListItems;
            PagingInfo = _PagingInfo;
        }  

        public static async Task<PaginatedList<T>> CreateDummyData(int page, List<T> items, int itemPerPage)
        {
            int pageSize = itemPerPage;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = page == 0 ? 1 : page;
            pagingInfo.TotalItems =  items.Count();
            pagingInfo.ItemsPerPage = pageSize;
            var skip = pageSize * (Convert.ToInt32(page) - 1);
            List<T> _ListItems =  items.Skip(skip).Take(pageSize).ToList();
            return new PaginatedList<T>(_ListItems, pagingInfo);
        }
    }
}
