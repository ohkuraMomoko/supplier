using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 商品規格
    /// </summary>
    public class ProductSpecsTypeDtoModel
    {
        /// <summary>
        /// 商品規格id
        /// </summary>
        [JsonProperty("Id")]
        public string SpecsId { get; set; }   //Id

        /// <summary>
        /// 商品規格名稱
        /// </summary>
        [JsonProperty("Title")]    ///Title
        public string SpecsName { get; set; }

        /// <summary>
        /// 商品規格庫存數量
        /// </summary>
        [JsonProperty("StockAmount")]   //StockAmount
        public string SpecsInstock { get; set; }

        /// <summary>
        /// 商品規格庫存數量(暫存紀錄用)
        /// </summary>
        [JsonProperty("SpecsInstockHidden")]   //StockAmount
        public string SpecsInstockHidden { get; set; }
    }
}