using Newtonsoft.Json;
using SupplierPlatform.Models.Dto;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 產品利率對照
    /// </summary>
    public class ProductPeriodViewModel : ResultViewModel
    {
        /// <summary>
        /// 分期利率清單
        /// </summary>
        [JsonProperty("EP_RATE_LIST_1")]
        public List<ProductPeriodDtoModel> Rates1 { get; set; }

        /// <summary>
        /// 分期利率清單
        /// </summary>
        [JsonProperty("EP_RATE_LIST_2")]
        public List<ProductPeriodDtoModel> Rates2 { get; set; }
    }
}