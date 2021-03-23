using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 系統公告 ViewModel
    /// </summary>
    public class LoginInfoViewModel
    {
        /// <summary>
        /// 系統公告
        /// </summary>
        [JsonProperty("LOGIN_INFO")]
        public string LoginInfo { get; set; }
    }
}