using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 登入檢核 ViewModel
    /// </summary>
    public class LoginViewModel : LoginBaseModel
    {
        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        [JsonProperty("PWD")]
        public string Secret { get; set; }

        /// <summary>
        /// 驗證碼
        /// </summary>
        [Required]
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 供應商 MEMBER ID
        /// </summary>
        [JsonProperty("MEMBER_ID")]
        public string MemberId { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty("TOKEN")]
        public string Token { get; set; }
    }
}