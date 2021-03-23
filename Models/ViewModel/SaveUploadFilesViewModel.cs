using Newtonsoft.Json;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 補件上傳
    /// </summary>
    public class SaveUploadFilesViewModel
    {
        /// <summary>
        /// 0正常 -1系統發生錯誤
        /// </summary>
        [JsonProperty("RTN_CD")]
        public int RtnCd { get; set; }

        /// <summary>
        /// 提示訊息
        /// </summary>
        [JsonProperty("ALERT_MSG")]
        public string AlertMsg { get; set; }
    }
}