using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 線上填單 ViewModel
    /// </summary>
    public class CaseFillInViewModel : UserInfoBaseModel
    {
        /// <summary>
        /// 訂單編號(訂單號碼若留空系統將自行產生訂單編號)
        /// </summary>
        [JsonProperty("ORDER_ID")]
        public string OrderId { get; set; }

        /// <summary>
        /// 經辦人員
        /// </summary>
        [JsonProperty("VENDER_SALES_NME")]
        public string Manager { get; set; }

        /// <summary>
        /// 經辦人員手機
        /// </summary>
        [JsonProperty("VENDER_SALES_MOBILE")]
        public string ManagerPhone { get; set; }

        /// <summary>
        /// Email寄送輸入
        /// </summary>
        [JsonProperty("CUST_EMAIL")]
        public string Email { get; set; }

        /// <summary>
        /// 商品類型
        /// </summary>
        [JsonProperty("EC_PROD_TYPE_CD")]
        [Required]
        public int ProductType { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [JsonProperty("EC_PROD_NME")]
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        /// 利率別：1:零利率 2:低利率 二選一
        /// </summary>
        [JsonProperty("EC_RATE_ID")]
        public int InterestRate { get; set; }

        /// <summary>
        /// 申請金額(標的物總金額/貸款本金) (是指UI的交易金額)
        /// </summary>
        [JsonProperty("OBJ_AMT")]
        [Required]
        public int TransAmt { get; set; }

        /// <summary>
        /// 分期期數
        /// </summary>
        [JsonProperty("PRD_NUM")]
        [Required]
        public int PrdNum { get; set; }

        /// <summary>
        /// 第一期款
        /// </summary>
        [JsonProperty("FIRST_PRD_AMT")]
        public int FirstPrdAmount { get; set; }

        /// <summary>
        /// 其餘每期款
        /// </summary>
        [JsonProperty("OTH_PRD_AMT")]
        public int OtherPrdAmount { get; set; }

        /// <summary>
        /// 分期總價款(每期款* 期數)
        /// </summary>
        [JsonProperty("TTL_AMT")]
        public int TotalAmount { get; set; }

        /// <summary>
        /// 分期付款利率(名目利率)
        /// </summary>
        [JsonProperty("APP_RATE")]
        public decimal AppRate { get; set; }

        /// <summary>
        /// 申請網頁網址寄送給消費者(1.簡訊代寄　2.Email代寄　)
        /// </summary>
        [JsonProperty("APP_URL_SEND_CUST")]
        public int SendMethod { get; set; }

        /// <summary>
        /// 手機號碼(簡訊寄送輸入)
        /// </summary>
        [JsonProperty("CUST_MOBILE")]
        public string Phone { get; set; }
    }
}