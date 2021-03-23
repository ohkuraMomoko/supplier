using Newtonsoft.Json;
using SupplierPlatform.Enums;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 商品開放期數
    /// </summary>
    public class ProductPeriodTypeDtoModel
    {
        /// <summary>
        /// 開放期數id
        /// </summary>
        [JsonProperty("period_id")]
        public string PeriodId { get; set; }

        /// <summary>
        /// 開放期數類型
        /// </summary>
        [JsonProperty("period_type")]
        public ProductOptionEnum PeriodType { get; set; }

        /// <summary>
        /// 開放期數數字
        /// </summary>
        [JsonProperty("period")]
        public string PeriodNum { get; set; }

        /// <summary>
        /// 分期數描述
        /// </summary>
        [JsonProperty("period_description")]
        public string PeriodDescription { get; set; }
    }

    /// <summary>
    /// 商品開放期數
    /// </summary>
    public class ProductPeriodTypeForPublish
    {
        /// <summary>
        /// 開放期數id
        /// </summary>
        public string PeriodId { get; set; }

        /// <summary>
        /// 開放期數類型
        /// </summary>
        public ProductOptionEnum PeriodType { get; set; }

        /// <summary>
        /// 開放期數數字
        /// </summary>
        public string PeriodNum { get; set; }

        /// <summary>
        /// 分期數描述
        /// </summary>
        public string PeriodDescription { get; set; }
    }
}