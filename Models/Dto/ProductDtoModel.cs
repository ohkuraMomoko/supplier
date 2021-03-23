using Newtonsoft.Json;
using SupplierPlatform.Enums;
using System.Collections.Generic;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 商品基底資訊
    /// </summary>
    public class ProductDtoModel
    {
        /// <summary>
        /// 商品編號
        /// </summary>
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        /// <summary>
        /// 開放期數
        /// </summary>
        [JsonProperty("product_period")]
        public List<ProductPeriodTypeDtoModel> ProductPeriod { get; set; }

        /// <summary>
        /// 建議售價
        /// </summary>
        [JsonProperty("Suggest_Price")]
        public string SuggestPrice { get; set; }

        /// <summary>
        /// 商品價格
        /// </summary>
        [JsonProperty("product_price")]
        public string ProductPrice { get; set; }

        /// <summary>
        /// 產品描述
        /// </summary>
        [JsonProperty("product_info")]
        public string ProductInfo { get; set; }

        /// <summary>
        /// 商品連結
        /// </summary>
        [JsonProperty("product_link")]
        public string ProductLink { get; set; }

        /// <summary>
        /// 產品商品介紹類型
        /// </summary>
        [JsonProperty("product_type")]
        public int ProductType { get; set; }

        /// <summary>
        /// 商品圖片
        /// </summary>
        //[JsonProperty("product_image")]
        //public string ProductImage { get; set; }

        /// <summary>
        /// 商品規格
        /// </summary>
        [JsonProperty("product_specs")]
        public List<ProductSpecsTypeDtoModel> ProductSpecs { get; set; }

        /// <summary>
        /// 商品庫存狀態
        /// </summary>
        public string InventoryStatus { get; set; }

        /// <summary>
        /// 商品圖片uuid.
        /// </summary>
        [JsonProperty("product_fileuuid")]
        public string ProductFileuuid { get; set; }

        /// <summary>
        /// 修改或新增的帳號.
        /// </summary>
        [JsonProperty("create_account")]
        public string CreateAccount { get; set; }

        /// <summary>
        /// 商店ID.
        /// </summary>
        [JsonProperty("storeid")]
        public string StoreId { get; set; }

        /// <summary>
        /// 商品狀態：0: 刪除 1: 下架 2: 上架
        /// </summary>
        [JsonProperty("product_status")]
        public ProductStatusEnum ProductStatus { get; set; }

        /// <summary>
        /// 自訂選項 custome_specs
        /// </summary>
        [JsonProperty("custome_specs")]
        public List<ProductCustomeSpecsTypeDtoModel> CustomeSpecs { get; set; }

        /// <summary>
        /// 商品圖片
        /// </summary>
        [JsonProperty("product_images")]
        public List<StoreProductImageDtoModel> ProductImages { get; set; }
    }
}