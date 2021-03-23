using Newtonsoft.Json;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 一頁式商品的分期數
    /// </summary>
    public class PeriodInfoViewModel
    {
        /// <summary>
        /// 分期數項目
        /// </summary>
        [JsonProperty("period_item")]
        public List<PeriodItemViewModel> PeriodItem { get; set; }
    }
}