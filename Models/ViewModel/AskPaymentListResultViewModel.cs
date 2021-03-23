using Newtonsoft.Json;
using SupplierPlatform.Models.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 顯示線上請款列表 ResultViewModel
    /// </summary>
    public class AskPaymentListResultViewModel : ResultViewModel
    {
        /// <summary>
        /// 案件明細列表
        /// </summary>
        [JsonProperty("CASE_LIST")]
        public List<AskPaymentCaseDtoModel> Cases { get; set; }
    }
}