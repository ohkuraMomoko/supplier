using Newtonsoft.Json;

namespace SupplierPlatform.Models
{
    /// <summary>
    /// 撥款概況結果
    /// </summary>
    public class AppropriationDataModel
    {
        /// <summary>
        /// 撥款日期
        /// </summary>
        [JsonProperty("DSB_DT")]
        public string AppropriationDate { get; set; }

        /// <summary>
        /// 筆數
        /// </summary>
        [JsonProperty("CASE_COUNT")]
        public string CaseCount { get; set; }

        /// <summary>
        /// 撥款金額
        /// </summary>
        [JsonProperty("SUM_DSB_AMT")]
        public string AppropriationPrice { get; set; }
    }
}