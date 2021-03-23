using Newtonsoft.Json;
using SupplierPlatform.Models.Dto;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 商品資訊新增跟修改
    /// </summary>
    public class ProductViewModel : ProductDtoModel
    {
        /// <summary>
        /// 商品規格
        /// </summary>
        [JsonProperty(" product_specs")]
        public string[] ProductSpecs { get; set; }

        /// <summary>
        /// 修改或是編輯：0: 新增 1: 修改 （預設為0）
        /// </summary>
        [JsonProperty("action")]
        public int Action { get; set; }
    }
}