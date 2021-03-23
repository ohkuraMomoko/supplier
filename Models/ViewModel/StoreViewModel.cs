using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 商店資料
    /// </summary>
    public class StoreViewModel : StoreInfoViewModel
    {
        /// <summary>
        /// 修改或是編輯：0: 新增 1: 修改 （預設為0）
        /// </summary>
        [JsonProperty("action")]
        public int Action { get; set; }

        /// <summary>
        /// 建立的帳號
        /// </summary>
        [JsonProperty("create_account")]
        public string CreateAccount { get; set; }
    }
}