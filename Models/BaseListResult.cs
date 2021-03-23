using System.Collections.Generic;

namespace SupplierPlatform.Models
{
    /// <summary>
    /// 基底集合 Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseListResult<T>
    {
        /// <summary>
        ///  API 查詢資料
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// 基底 API 執行狀態及訊息
        /// </summary>
        public ResultModel Result { get; set; }
    }
}