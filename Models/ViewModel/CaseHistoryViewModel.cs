using Newtonsoft.Json;
using System;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 案件歷程 View Model
    /// </summary>
    public class CaseHistoryViewModel
    {
        /// <summary>
        /// 留言者：1: 客服, 2: 供應商
        /// </summary>
        [JsonProperty("who")]
        public int Who { get; set; }

        /// <summary>
        /// 回覆訊息
        /// </summary>
        [JsonProperty("reply")]
        public string Reply { get; set; }

        /// <summary>
        /// 留言時間
        /// </summary>
        [JsonProperty("reply_date")]
        public DateTime ReplyDt { get; set; }
    }
}