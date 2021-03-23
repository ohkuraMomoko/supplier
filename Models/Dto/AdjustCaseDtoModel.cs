using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    public class AdjustCaseDtoModel : AdjustCaseBaseDtoModel
    {
        /// <summary>
        /// 執行狀態：0正常 -1錯誤
        /// </summary>
        public int RTN_CD { get; set; }

        /// <summary>
        /// 提示訊息
        /// </summary>
        public string ALERT_MSG { get; set; }

        /// <summary>
        /// 狀態(Ex:已撥款)
        /// </summary>
        [JsonProperty("STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// 業種別
        /// </summary>
        [JsonProperty("BUSN_TYP")]
        public string BusnType { get; set; }

        /// <summary>
        /// 廠商 ID
        /// </summary>
        [JsonProperty("Vonder_ID")]
        public string VondorId { get; set; }

        /// <summary>
        /// 撥款日
        /// </summary>
        [JsonProperty("DSB_DT")]
        public string AppropriationDate { get; set; }

        /// <summary>
        /// 標的物金額(案件金額)
        /// </summary>
        [JsonProperty("OBJ_TTL_AMT")]
        public int TransactionAmount { get; set; }

        /// <summary>
        /// 保證金
        [JsonProperty("GUAR_AMT")]
        public int EarnestMoney { get; set; }

        /// <summary>
        /// 作業處理費
        /// </summary>
        [JsonProperty("OP_AMT")]
        public int OperationAmount { get; set; }

        /// <summary>
        /// 上次付款日
        /// </summary>
        [JsonProperty("LAST_PAY_DT")]
        public string LastPaymentDt { get; set; }
    }
}