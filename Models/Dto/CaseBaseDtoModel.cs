using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    public class CaseBaseDtoModel
    {
        /// <summary>
        /// 申請日期
        /// </summary>
        [JsonProperty("CREATE_DT")]
        public string CreateDt { get; set; }

        /// <summary>
        /// 門市名稱
        /// </summary>
        [JsonProperty("VENDER_NME")]
        public string StoreName { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [JsonProperty("ORDER_ID")]
        public string OrderId { get; set; }

        /// <summary>
        /// 案件編號(契約編號)
        /// </summary>
        [JsonProperty("APP_NO")]
        public string AppNo { get; set; }

        /// <summary>
        /// 客戶名稱(線上請款)/申請人姓名(最新案件)
        /// </summary>
        [JsonProperty("CUST_NME")]
        public string CustName { get; set; }

        /// <summary>
        /// 消費金額(線上請款)/交易金額(最新案件)
        /// </summary>
        [JsonProperty("OBJ_TTL_AMT")]
        public string TransactionAmount { get; set; }

        /// <summary>
        /// 期數
        /// </summary>
        [JsonProperty("PRD_NUM")]
        public string Period { get; set; }

        /// <summary>
        /// 商家編號
        /// </summary>
        [JsonProperty("MERCHANT_ID")]
        public string MerchantId { get; set; }
    }
}