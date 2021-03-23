using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 分期利率明細
    /// </summary>
    public class PeriodNumInformationDtoModel
    {
        /// <summary>
        /// 期數
        /// </summary>
        [JsonProperty("PRD_NUM")]
        public string PeriodNum { get; set; }

        /// <summary>
        /// 利率(%)
        /// </summary>
        [JsonProperty("FEE_RATE")]
        public string InterestRate { get; set; }
    }
}