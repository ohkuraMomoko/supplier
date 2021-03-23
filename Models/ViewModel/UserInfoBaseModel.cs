using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 會員基底資料
    /// </summary>
    public class UserInfoBaseModel
    {
        /// <summary>
        /// 頂層供應商ID
        /// </summary>
        [JsonProperty("TOP_VENDER_ID")]
        public string TopVenderId { get; set; }

        /// <summary>
        /// 底層供應商ID
        /// </summary>
        [JsonProperty("VENDER_ID")]
        public string VenderId { get; set; }

        /// <summary>
        /// 頂層供應商名稱
        /// </summary>
        [JsonProperty("TOP_VENDER_NME")]
        public string TopVenderName { get; set; }

        /// <summary>
        /// 底層供應商名稱
        /// </summary>
        [JsonProperty("VENDER_NME")]
        public string VenderName { get; set; }

        /// <summary>
        /// 廠商會員ID
        /// </summary>
        [JsonProperty("MEMBER_ID")]
        public int MemberId { get; set; }
    }
}