using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 退款相關 ViewModel
    /// </summary>
    public class CaseReturnViewModel
    {
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
        /// 解約金
        /// </summary>
        [JsonProperty("ABT_AMT")]
        public int TerminateAmount { get; set; }

        /// <summary>
        /// 手續費 
        /// </summary>
        [JsonProperty("HNDL_FEE")]
        public int HandlingFee { get; set; }

        /// <summary>
        /// 是否為改單(是:1 否:0)
        /// </summary>
        [JsonProperty("IS_CHG")]
        public string IsChange { get; set; }

        /// <summary>
        /// 調降後最終金額
        /// </summary>
        [JsonProperty("NEW_OBJ_AMT")]
        public int NewTransactionAmount { get; set; }

        /// <summary>
        /// 退款方式：1:撥款中扣除 2:廠商自行匯款
        /// </summary>
        [JsonProperty("CANCEL_METHOD")]
        public int CancelMethod { get; set; }

    }
}