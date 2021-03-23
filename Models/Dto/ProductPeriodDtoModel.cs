using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 產品利率
    /// </summary>
    public class ProductPeriodDtoModel
    {
        /// <summary>
        /// 期數
        /// </summary>
        [JsonProperty("PRD_NUM")]
        public string PeriodNum { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        [JsonProperty("CASE_RATE")]
        public string InterestRate { get; set; }
    }
}