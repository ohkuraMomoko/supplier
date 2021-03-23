using Newtonsoft.Json;
using SupplierPlatform.Extensions;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 最新案件、訂單/案件查詢 的案件明細
    /// </summary>
    public class CaseDtoModel : CaseBaseDtoModel
    {
        /// <summary>
        /// 更新日期
        /// </summary>
        [JsonProperty("UPD_DT")]
        public string UpdateDt { get; set; }

        /// <summary>
        /// 案件狀態 ID
        /// </summary>
        [JsonProperty("STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// 案件狀態 type
        /// </summary>
        public string StatusType
        {
            get => this.Status.ConvertLatestCaseStatus();
        }

        /// <summary>
        /// 一頁式電商狀態 type
        /// </summary>
        public string OnePageStatusType
        {
            get => this.Status.ConvertOnePageStatus();
        }

        /// <summary>
        /// 案件狀態
        [JsonProperty("STATUS_NME")]
        public string StatusName { get; set; }

        /// <summary>
        /// 申請人 ID
        /// </summary>
        [JsonProperty("CUST_ID")]
        public string CustId { get; set; }

        /// <summary>
        /// 經辦人員(EC_ORDER_DATA.VENDER_SALES_NME)
        /// </summary>
        [JsonProperty("VENDER_SALES_NME")]
        public string VenderSalesName { get; set; }

        /// <summary>
        /// 電子、紙本
        /// </summary>
        [JsonProperty("APLY_TYPE")]
        public string ApplyType { get; set; }

        /// <summary>
        /// EP_RESERVE、APP_FORM、CASE_DATA
        /// </summary>
        [JsonProperty("TYPE_ID")]
        public string TYPE_ID { get; set; }

        /// <summary>
        /// 供應商頂層群組 ID
        /// </summary>
        [JsonProperty("TOP_VENDER_ID")]
        public string TopVenderId { get; set; }

        /// <summary>
        /// 供應商 ID
        /// </summary>
        [JsonProperty("VENDER_ID")]
        public string VenderId { get; set; }

        /// <summary>
        /// 送件編號：CASE_ID、EC_CASE_ID、EP_ CASE_ID
        /// </summary>
        [JsonProperty("CASE_ID")]
        public string CaseId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [JsonProperty("PRODUCT_NAME")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品規格
        /// </summary>
        [JsonProperty("Product_Specs")]
        public string ProductSpecs { get; set; }

        /// <summary>
        /// 自定義規格
        /// </summary>
        [JsonProperty("Custome_Specs")]
        public string CustomeSpecs { get; set; }

        /// <summary>
        /// 撥款金額
        /// </summary>
        [JsonProperty("DSB_AMT")]
        public string AppropriationAmount { get; set; }

        /// <summary>
        /// 消費者通訊地址
        /// </summary>
        [JsonProperty("ADDR")]
        public string Address { get; set; }

        /// <summary>
        /// 消費者手機
        /// </summary>
        [JsonProperty("MOBILE")]
        public string Mobile { get; set; }
    }
}