using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 客服留言
    /// </summary>
    public class CustomerMessageViewModel
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
        /// 留言時間：格式 YYYY/MM/DD HH:mm
        /// </summary>
        [JsonProperty("reply_date")]
        public string ReplyDt { get; set; }

        /// <summary>
        /// 序列編號
        /// </summary>
        [JsonProperty("sort")]
        public int Sort { get; set; }
    }
}