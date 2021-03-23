using Newtonsoft.Json;
using SupplierPlatform.Models.Dto;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 分期利率清單 ViewModel
    /// </summary>
    public class PeriodNumInformationViewModel : ResultViewModel
    {
        /// <summary>
        /// 分期利率清單
        /// </summary>
        [JsonProperty("CASE_LIST")]
        public List<PeriodNumInformationDtoModel> Cases { get; set; }
    }
}