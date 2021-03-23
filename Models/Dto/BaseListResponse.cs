using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplierPlatform.Models.Dto
{
    public class BaseListResponse<T>
    {
        /// <summary>
        ///  API 查詢資料
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// 基底 API 執行狀態及訊息
        /// </summary>
        public Result Result { get; set; }
    }
}