using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 商品明細
    /// </summary>
    public class ProductInformationDtoModel
    {
        /// <summary>
        /// 電子填單商品類型代碼
        /// </summary>
        [JsonProperty("EC_PROD_TYPE_CD")]
        public string ProductType { get; set; }

        /// <summary>
        /// 商品類型
        /// </summary>
        [JsonProperty("EC_PROD_TYPE_NME")]
        public string ProductTypeName { get; set; }

        /// <summary>
        /// 利率別 1 零利率 2 低利率
        /// </summary>
        [JsonProperty("EC_RATE_ID")]
        public int RateType { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [JsonProperty("EC_PROD_NME")]
        public string ProductName { get; set; }

        /// <summary>
        /// 異動(生效)日
        /// </summary>
        [JsonProperty("EC_QUOTE_CHG_DT")]
        public string UpdateDt { get; set; }

        /// <summary>
        /// 批覆書主檔MAJ_SEQ_ID
        /// </summary>
        [JsonProperty("EC_QUOTE_MAJ_SEQ_ID")]
        public int MajSeqId { get; set; }

        /// <summary>
        /// 項目流水號
        /// </summary>
        [JsonProperty("EC_QUOTE_SUB_SEQ_ID")]
        public int SeqId { get; set; }
    }
}