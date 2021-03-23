using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 商店資料
    /// </summary>
    public class StoreInfoViewModel
    {
        /// <summary>
        /// 商店ID
        /// </summary>
        [JsonProperty("storeid")]
        public string Storeid { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        [JsonProperty("storename")]
        public string StoreName { get; set; }

        /// <summary>
        /// 大標題
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 小標題
        /// </summary>
        [JsonProperty("subtitle")]
        public string SubTitle { get; set; }

        /// <summary>
        /// 聯絡地址
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Line Id
        /// </summary>
        [JsonProperty("lineid")]
        public string LineId { get; set; }

        /// <summary>
        /// FB粉絲團網址
        /// </summary>
        [JsonProperty("facebookid")]
        public string FacebookId { get; set; }

        /// <summary>
        /// 商點網站
        /// </summary>
        [JsonProperty("storeurl")]
        public string StoreUrl { get; set; }

        /// <summary>
        /// 商家IG網址
        /// </summary>
        [JsonProperty("igurl")]
        public string IGUrl { get; set; }

        /// <summary>
        /// 聯絡電話：手機號碼或市話, 須檢測手機格式。/((?=(0))[0-9]{10})$/
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 配送說明
        /// </summary>
        [JsonProperty("delivery_note")]
        public string DeliveryNote { get; set; }

        /// <summary>
        /// 其他說明
        /// </summary>
        [JsonProperty("other_instructions")]
        public string OtherInstructions { get; set; }

        /// <summary>
        /// Logo路徑
        /// </summary>
        [JsonProperty("logo_Path")]
        public string LogoPath { get; set; }

        /// <summary>
        /// 檔案識別碼
        /// </summary>
        [JsonProperty("logo_fileuuid")]
        public string LogoFileuuid { get; set; }

        /// <summary>
        /// Merchant Id
        /// </summary>
        [JsonProperty("Merchant_Id")]
        public string MerchantId { get; set; }

        /// <summary>
        /// API Key
        /// </summary>
        [JsonProperty("API_Key")]
        public string ApiKey { get; set; }

        /// <summary>
        /// 商店發布狀態
        /// </summary>
        [JsonProperty("DeployStatus")]
        public string DeployStatus { get; set; }

    }
}