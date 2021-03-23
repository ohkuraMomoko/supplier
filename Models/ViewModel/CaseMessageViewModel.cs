using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 案件留言
    /// </summary>
    public class CaseMessageViewModel
    {
        /// <summary>
        /// 留言角色：零卡(系統、經辧)
        /// </summary>
        [JsonProperty("ROLE")]
        public string Role { get; set; }

        /// <summary>
        /// 歷程內容
        /// </summary>
        [JsonProperty("RECORD_CNT")]
        public string RecodeContent { get; set; }

        /// <summary>
        /// 歷程時間 YYYY/MM/DD HH24:MI:SS
        /// </summary>
        [JsonProperty("UPD_DT")]
        public string UpdateDt { get; set; }

        /// <summary>
        /// 留言ID
        /// </summary>
        [JsonProperty("UPD_USR_ID")]
        public string UpdateId { get; set; }
    }
}