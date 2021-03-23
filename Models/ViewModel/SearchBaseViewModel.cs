using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 搜尋相關基底
    /// </summary>
    public class SearchBaseViewModel
    {
        /// <summary>
        /// 搜尋類型：1：訂單編號。2：身分證字號。3：姓名。4：案件編號。5：車牌號碼。
        /// </summary>
        [JsonProperty("search_type")]
        public int SearchType { get; set; }

        /// <summary>
        /// 搜尋內容
        /// </summary>
        [JsonProperty("search")]
        public string Search { get; set; }
    }
}