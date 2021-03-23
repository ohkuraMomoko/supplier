using Newtonsoft.Json;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 門市或據點資訊
    /// </summary>
    public class StoreInformationViewModel
    {
        /// <summary>
        /// 層級(1.總公司,2.門市, 據點)
        /// </summary>
        [JsonProperty("LVL")]
        public string Level { get; set; }

        /// <summary>
        /// 供應商ID
        /// </summary>
        [JsonProperty("VENDER_ID")]
        public string VenderId { get; set; }

        /// <summary>
        /// 供應商名稱
        /// </summary>
        [JsonProperty("VENDER_NME")]
        public string VenderName { get; set; }

        /// <summary>
        /// 門市代碼
        /// </summary>
        [JsonProperty("VNO")]
        public string VNO { get; set; }

        /// <summary>
        /// 第二跟第三層 (門市或據點資訊)
        /// </summary>
        [JsonProperty("VEDNER_LIST")]
        public List<StoreInformationViewModel> Venders { get; set; }
    }
}