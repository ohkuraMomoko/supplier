using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 基底查詢欄位 DtoModel
    /// </summary>
    public class BillingSearchBaseDtoModel
    {
        /// <summary>
        /// 供應商帳號MEMBER_ID
        /// </summary>
        [JsonProperty("MEMBER_ID")]
        public int MemberId { get; set; }

        /// <summary>
        /// 申請日 開始日期(YYYYMMDD)
        /// </summary>
        [JsonProperty("START_DT")]
        public string StartDt { get; set; }

        /// <summary>
        /// 申請日 結束日期(YYYYMMDD)
        /// </summary>
        [JsonProperty("END_DT")]
        public string EndDt { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [JsonProperty("ORDER_ID")]
        public string OrderId { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        [JsonProperty("APP_NO")]
        public string AppNo { get; set; }
    }
}