using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 通知清單內容 ViewModel
    /// </summary>
    public class NotificationViewModel
    {
        /// <summary>
        /// 廠商會員ID
        /// </summary>
        [JsonProperty("MEMBER_ID")]
        public string MemberId { get; set; }

        /// <summary>
        /// 分頁指標，無輸入為全找
        /// </summary>
        [JsonProperty("INDEX")]
        public int? Index { get; set; }

        /// <summary>
        /// 分頁指標，無輸入為全找
        /// </summary>
        [JsonProperty("PAGE_NUM")]
        public int? PageNumber { get; set; }
    }
}