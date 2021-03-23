using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 撥款、暗降相關基底欄位
    /// </summary>
    public class BillingBaseViewModel : TransactionBaseViewModel
    {
        /// <summary>
        /// 申請人(顯示前6字元)
        /// </summary>
        [Display(Name = "申請人")]
        [JsonProperty("applicant")]
        public string Applicant { get; set; }
    }
}