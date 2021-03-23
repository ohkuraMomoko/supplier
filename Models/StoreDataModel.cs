using Newtonsoft.Json;

namespace SupplierPlatform.Models
{
    /// <summary>
    /// 門市資料
    /// </summary>
    public class StoreDataModel
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty("VNO2")]
        public string Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [JsonProperty("VNAME")]
        public string Name { get; set; }
    }
}