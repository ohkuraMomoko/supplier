using Newtonsoft.Json;

namespace SupplierPlatform.Models
{
    public class CaseTypeDtoModel
    {
        /// <summary>
        /// 狀態描述
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 狀態類型：1: 紙本 2.電子
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }
    }
}