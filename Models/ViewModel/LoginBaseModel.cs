using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 登入相關基底 Model
    /// </summary>
    public class LoginBaseModel
    {
        /// <summary>
        /// 登入統編
        /// </summary>
        [Required]
        [JsonProperty("VENDNO")]
        public string UniformNumbers { get; set; }

        /// <summary>
        /// 帳號(手機號碼)
        /// </summary>
        [Required]
        [JsonProperty("MOBILE_NO")]
        public string Phone { get; set; }
    }
}