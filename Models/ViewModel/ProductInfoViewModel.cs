using Newtonsoft.Json;
using SupplierPlatform.Enums;
using SupplierPlatform.Models.Dto;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 商品資訊
    /// </summary>
    public class ProductInfoViewModel : ProductDtoModel
    {
        /// <summary>
        /// 商品主圖片
        /// </summary>
        [JsonProperty("product_image")]
        public string ProductImage { get; set; }

        /// <summary>
        /// 商品圖文類別
        /// </summary>
        [JsonProperty("product_IntroductType")]
        public string productIntroductType { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
    }
}