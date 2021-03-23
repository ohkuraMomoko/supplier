using Newtonsoft.Json;

namespace SupplierPlatform.Models
{
    public class PageListResult<T> : BaseListResult<T>
    {
        /// <summary>
        /// 總頁數
        /// </summary>
        [JsonProperty("total_num")]
        public int TotalNum { get; set; }
    }
}