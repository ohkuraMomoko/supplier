using Newtonsoft.Json;
using SupplierPlatform.Models.Dto;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 取得調降金額案件資訊
    /// </summary>
    public class AdjustCaseInfoViewModel : ResultViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("ITEM")]
        public AdjustCaseDtoModel Item { get; set; }
    }
}