using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    public class AppropriationInquiryCaseDtoModel : CaseBaseDtoModel
    {
        /// <summary>
        /// 撥款日期
        /// </summary>
        [JsonProperty("DSB_DT")]
        public string AppropriationDate { get; set; }

        /// <summary>
        /// 利率別
        /// </summary>
        [JsonProperty("FEE_TYPE")]
        public string InterestRateType { get; set; }

        /// <summary>
        /// 撥款金額
        /// </summary>
        [JsonProperty("DSB_AMT")]
        public string AppropriationAmount { get; set; }
    }
}