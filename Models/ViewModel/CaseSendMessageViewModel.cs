using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 案件留言發送
    /// </summary>
    public class CaseSendMessageViewModel : ResultViewModel
    {
        /// <summary>
        /// 案件狀態 ID
        /// </summary>
        [JsonProperty("STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// 錯誤訊息(app用,前端web不要用)
        /// </summary>
        [JsonProperty("ERROR_MSG")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// object(app用,前端web不要用)
        /// </summary>
        [JsonProperty("ITEM")]
        public string Item { get; set; }
    }
}