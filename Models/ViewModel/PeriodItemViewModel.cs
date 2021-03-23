using Newtonsoft.Json;
using SupplierPlatform.Enums;

namespace SupplierPlatform.Models.ViewModel
{
    public class PeriodItemViewModel
    {
        /// <summary>
        /// 分期數類型
        /// </summary>
        [JsonProperty("period_type")]
        public ProductOptionEnum PeriodType { get; set; }

        /// <summary>
        /// 分期數描述
        /// </summary>
        [JsonProperty("period_description")]
        public string PeriodDescription { get; set; }

        /// <summary>
        /// 分期數
        /// </summary>
        [JsonProperty("period")]
        public int Period { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        [JsonProperty("period")]
        public double Rate { get; set; }
    }
}