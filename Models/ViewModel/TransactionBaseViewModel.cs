using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 撥款、訂單等相關基底欄位
    /// </summary>
    public class TransactionBaseViewModel
    {
        /// <summary>
        /// 申請日期(線上填單申請的日期)
        /// </summary>
        [JsonProperty("date")]
        [Display(Name = "申請日期")]
        public string Date { get; set; }

        /// <summary>
        /// 門市(顯示前6個字)
        /// </summary>
        [Display(Name = "門市")]
        [JsonProperty("store")]
        public string Store { get; set; }

        /// <summary>
        /// 訂單編號(顯示前14碼)
        /// </summary>
        [Display(Name = "訂單編號")]
        [JsonProperty("order_sn")]
        public string OrderSn { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        [Display(Name = "案件編號")]
        [JsonProperty("case_sn")]
        public string CaseSn { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Display(Name = "交易金額")]
        [JsonProperty("price")]
        public string Price { get; set; }

        /// <summary>
        /// 期數(申請時設定值)
        /// </summary>
        [Display(Name = "期數")]
        [JsonProperty("period")]
        public string Period { get; set; }
    }
}