using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 訂單/案件 resule ViewModel
    /// </summary>
    public class OrderInquiryResultViewModel : BillingBaseViewModel
    {
        /// <summary>
        /// 經辦人員(顯示前6字元)
        /// </summary>
        [JsonProperty("person_in_charge")]
        public string PersonInCharge { get; set; }

        /// <summary>
        /// 案件狀態
        /// </summary>
        [Display(Name = "案件狀態")]
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// 案件類型
        /// </summary>
        [Display(Name = "案件類型")]
        [JsonProperty("case_type")]
        public CaseTypeDtoModel CaseType { get; set; }
    }
}