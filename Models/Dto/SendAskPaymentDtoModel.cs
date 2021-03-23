using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 線上請款 Dto Model
    /// </summary>
    public class SendAskPaymentDtoModel
    {
        /// <summary>
        /// 廠商會員ID
        /// </summary>
        [JsonProperty("MEMBER_ID")]
        public int MemberId { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        [JsonProperty("APP_NO")]
        public string AppNo { get; set; }

        /// <summary>
        /// 商家編號
        /// </summary>
        [JsonProperty("MERCHANT_ID")]
        public string MerchantId { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [JsonProperty("ORDER_ID")]
        public string OrderId { get; set; }

        /// <summary>
        /// 來源站台
        /// </summary>
        [JsonProperty("CHANNEL")]
        public string Channel { get; set; }
    }
}