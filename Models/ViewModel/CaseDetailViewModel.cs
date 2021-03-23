using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 案件明細 ViewModel
    /// </summary>
    public class CaseDetailViewModel
    {
        /// <summary>
        /// 供應商帳號 MEMBER_ID
        /// </summary>
        [JsonProperty("MEMBER_ID")]
        public int MemberId { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [JsonProperty("ORDER_ID")]
        public string OrderId { get; set; }

        /// <summary>
        /// 案件編號(契約編號)
        /// </summary>
        [JsonProperty("APP_NO")]
        public string CaseNumber { get; set; }

        /// <summary>
        /// 商家編號
        /// </summary>
        [JsonProperty("MERCHANT_ID")]
        public string MerchantId { get; set; }
    }
}