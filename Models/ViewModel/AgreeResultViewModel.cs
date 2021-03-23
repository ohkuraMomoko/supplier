using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 會員條款 ResultViewModel
    /// </summary>
    public class AgreeResultViewModel
    {
        /// <summary>
        /// 登入條款
        /// </summary>
        [JsonProperty("AGREEMENT")]
        public string Agreement { get; set; }
    }
}