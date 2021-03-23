using System.Collections.Generic;

namespace SupplierPlatform.Models
{
    /// <summary>
    /// 基底M回覆 Model
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class BaseResult<T>
    {
        /// <summary>
        ///  API 查詢資料
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 基底 API 執行狀態及訊息
        /// </summary>
        public ResultModel Result { get; set; }
    }    
}