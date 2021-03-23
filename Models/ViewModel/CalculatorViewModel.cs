using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 金額試算 ViewModel
    /// </summary>
    public class CalculatorViewModel
    {
        /// <summary>
        /// 利率別：1:零利率 2:低利率 二選一
        /// </summary>
        [JsonProperty("interest_rate")]
        public int InterestRate { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [JsonProperty("trans_amt")]
        [Required]
        public int TransAmt { get; set; }

        /// <summary>
        /// 分期期數
        /// </summary>
        [JsonProperty("prd_num")]
        [Required]
        public int PrdNum { get; set; }
    }
}