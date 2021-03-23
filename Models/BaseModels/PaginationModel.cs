using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplierPlatform.Models.BaseModels
{
    public class PaginationModel
    {
        /// <summary>
        /// 目前頁數。(從1開始)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每頁幾筆資料，預設為10。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總共資料數。
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 總共頁數。
        /// </summary>
        public int TotalPage { get; set; }
    }
}