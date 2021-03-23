using Newtonsoft.Json;
using SupplierPlatform.Models.Dto;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 商品清單 ViewModel
    /// </summary>
    public class ProductInformationViewModel : ResultViewModel
    {
        /// <summary>
        /// 案件狀態
        /// </summary>
        [JsonProperty("ITEM_LIST")]
        public List<ProductInformationDtoModel> Items { get; set; }
    }
}