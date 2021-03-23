using Newtonsoft.Json;
using SupplierPlatform.Models.Dto;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 查詢撥款 ViewModel
    /// </summary>
    public class AppropriationInquiryResultViewModel : ResultViewModel
    {
        /// <summary>
        /// 查詢結果
        /// </summary>
        [JsonProperty("CASE_LIST")]
        public List<AppropriationInquiryCaseDtoModel> Cases { get; set; }

        /// <summary>
        /// 撥款概況
        /// </summary>
        [JsonProperty("SUM_LIST")]
        public List<AppropriationDataModel> Sums { get; set; }
    }
}