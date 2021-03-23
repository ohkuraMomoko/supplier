using Newtonsoft.Json;

namespace SupplierPlatform.Models
{
    /// <summary>
    /// Response 回覆訊息
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 狀態
        /// </summary>
        [JsonProperty("status")]
        public bool Status { get; set; }

        /// <summary>
        /// 成功或失敗的訊息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 狀態碼
        /// </summary>
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }
}