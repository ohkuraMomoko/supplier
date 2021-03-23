using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 商品規格及庫存 Dto Model
    /// </summary>
    public class SpecDtoModel
    {
        /// <summary>
        /// 選項編號
        /// </summary>
        [JsonProperty("Id")]
        public int Id { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        [JsonProperty("Product_Id")]
        public int Product_Id { get; set; }

        /// <summary>
        /// 規格名稱
        /// </summary>
        [JsonProperty("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 庫存數量
        /// </summary>
        [JsonProperty("StockAmount")]
        public string StockAmount { get; set; }

        /// <summary>
        /// 商品庫存狀態： 0 刪除   1使用
        /// </summary>
        [JsonProperty("Status")]
        public string Status { get; set; }
    }
}