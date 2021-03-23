using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 通知中心狀態變更 Dto Model
    /// </summary>
    public class NotificationStatusDtoModel
    {
        /// <summary>
        /// 廠商會員ID
        /// </summary>
        [JsonProperty("MEMBER_ID")]
        public string MemberId { get; set; }

        /// <summary>
        /// 通知訊息編號無輸入則更新全部
        /// </summary>
        [JsonProperty("MAJ_SEQ_ID")]
        public string MajSeqId { get; set; }

        /// <summary>
        /// 訊息狀態： "R".已讀, "D" 刪除
        /// </summary>
        [JsonProperty("STATUS")]
        public string Status { get; set; }
    }
}