using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 調降金額、退貨 基底 MODEL
    /// </summary>
    public class AdjustCaseBaseDtoModel
    {
        /// <summary>
        /// 期數
        /// </summary>
        [JsonProperty("PRD_NUM")]
        public int Period { get; set; }

        /// <summary>
        /// 撥款金額
        /// </summary>
        [JsonProperty("DSB_AMT")]
        public int AppropriationAmount { get; set; }

        /// <summary>
        /// 頂層供應商 ID
        /// </summary>
        [JsonProperty("TOP_VENDER_ID")]
        public string TopVondorId { get; set; }

        /// <summary>
        /// 客戶已繳金額
        /// </summary>
        [JsonProperty("PAID_AMT")]
        public int PaidAmount { get; set; }

        /// <summary>
        /// 管理費
        /// </summary>
        [JsonProperty("MA_AMT")]
        public int ManagementAmount { get; set; }

        /// <summary>
        /// 解約計算距撥款期數
        /// </summary>
        [JsonProperty("DIFF_PRD_NUM")]
        public int TerminatePeriod { get; set; }

        /// <summary>
        /// 解約計算用本餘
        /// </summary>
        [JsonProperty("CAL_FEE_BAL")]
        public int TerminateBalance { get; set; }

        /// <summary>
        /// 距撥款日天數
        /// </summary>
        [JsonProperty("DIFF_DSB_DAYS")]
        public int DaysFromAppropriationDate { get; set; }

        /// <summary>
        /// 財顧費
        /// </summary>
        [JsonProperty("FN_AMT")]
        public int FinancialAdvisoryAmount { get; set; }

        /// <summary>
        /// 是否為機車(是:1 否:0)
        /// </summary>
        [JsonProperty("IS_MOTO")]
        public string IsMotorcycle { get; set; }

        /// <summary>
        /// 解約計算用付款金額
        /// </summary>
        [JsonProperty("CAL_PAY_AMT")]
        public int TerminatePaymentAmount { get; set; }
    }
}