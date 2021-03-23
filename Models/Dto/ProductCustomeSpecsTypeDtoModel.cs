using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 自訂商品規格
    /// </summary>
    public class ProductCustomeSpecsTypeDtoModel
    {
        /// <summary>
        /// 自訂商品規格id
        /// </summary>
        [JsonProperty("specs_id")]
        public string SpecsId { get; set; }

        /// <summary>
        /// 自訂商品規格名稱
        /// </summary>
        [JsonProperty("specs_name")]
        public string SpecsName { get; set; }
    }

    /// <summary>
    /// 自訂商品規格
    /// </summary>
    public class ProductCustomeSpecsTypeForPublish
    {
        /// <summary>
        /// 自訂商品規格id
        /// </summary>
        public string SpecsId { get; set; }

        /// <summary>
        /// 自訂商品規格名稱
        /// </summary>
        public string SpecsName { get; set; }
    }
}
