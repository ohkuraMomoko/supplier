using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 金額試算
    /// </summary>
    public class CalculatorResultViewModel
    {
        /// <summary>
        /// 首期付款
        /// </summary>
        [JsonProperty("Initial_payment")]
        public int InitialPayment { get; set; }

        /// <summary>
        /// 其他期款
        /// </summary>
        [JsonProperty("other_installments")]
        public int OtherInstallments { get; set; }

        /// <summary>
        /// 總價款
        /// </summary>
        [JsonProperty("total_price")]
        public int TotalPrice { get; set; }
    }
}