using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 修改密碼
    /// </summary>
    public class ChangePassWordViewModel
    {
        /// <summary>
        /// new_pwd
        /// </summary>
        [JsonProperty("MEMBER_ID")]
        public string MemberId { get; set; }

        /// <summary>
        /// 舊密碼
        /// </summary>
        [JsonProperty("OLD_PWD")]
        [Required]
        public string OldPwd { get; set; }

        /// <summary>
        /// 新密碼
        /// </summary>
        [JsonProperty("NEW_PWD")]
        [Required]
        public string NewPwd { get; set; }

    }
}