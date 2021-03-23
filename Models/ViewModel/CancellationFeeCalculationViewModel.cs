using Newtonsoft.Json;
using SupplierPlatform.Models.Dto;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 調降金額確認(解約金試算)
    /// </summary>
    public class CancellationFeeCalculationViewModel : AdjustCaseBaseDtoModel
    {
        /// <summary>
        /// 供應商帳號 MEMBER_ID
        /// </summary>
        [JsonProperty("MEMBER_ID")]
        public int MemberId { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        [JsonProperty("APP_NO")]
        public string AppNo { get; set; }

        /// <summary>
        /// 解約日 (yyyyMMdd) 放系統日期
        /// </summary>
        [JsonProperty("ABT_DT")]
        public string TerminateDt { get; set; }

        /// <summary>
        /// 是否為改單(是:1 否:0) 退貨 0  調降金額 1
        /// 案件金額 - 調降金額 = 0   => 退貨
        /// 案件金額 - 調降金額 > 0   =>  調降金額(改單
        /// </summary>
        [JsonProperty("IS_CHG")]
        public string IsChange { get; set; }

        /// <summary>
        /// 調降後最終金額
        /// </summary>
        [JsonProperty("NEW_OBJ_AMT")]
        public int NewTransactionAmount { get; set; }

        /// <summary>
        /// 保證金
        [JsonProperty("GUAR_AMT")]
        public int EarnestMoney { get; set; }
    }
}