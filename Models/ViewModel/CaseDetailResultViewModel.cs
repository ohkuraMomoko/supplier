using Newtonsoft.Json;
using SupplierPlatform.Extensions;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 案件詳細資訊
    /// </summary>
    public class CaseDetailResultViewModel : ResultViewModel
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        [JsonProperty("ORDER_ID")]
        public string OrderId { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        [JsonProperty("APP_NO")]
        public string CaseNumber { get; set; }

        /// <summary>
        /// 申請人ID
        /// </summary>
        [JsonProperty("CUST_ID")]
        public string CustId { get; set; }

        /// <summary>
        /// 案件狀態 ID
        /// </summary>
        [JsonProperty("STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// 案件狀態 ID
        /// </summary>
        public string StatusType
        {
            get => this.Status.ConvertLatestCaseStatus();
        }

        /// <summary>
        /// STATUS_NME
        /// </summary>
        [JsonProperty("STATUS_NME")]
        public string StatusName { get; set; }

        /// <summary>
        /// 申請日
        /// </summary>
        [JsonProperty("CREATE_DT")]
        public string ApplicationDate { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [JsonProperty("OBJ_TTL_AMT")]
        public string TransactionAmount { get; set; }

        /// <summary>
        /// 申請人姓名
        /// </summary>
        [JsonProperty("CUST_NME")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// 申請人電話
        /// </summary>
        [JsonProperty("MOBILE")]
        public string ApplicationPhone { get; set; }

        /// <summary>
        /// 期數
        /// </summary>
        [JsonProperty("PRD_NUM")]
        public string Period { get; set; }

        /// <summary>
        /// 第一期款 期付款
        /// </summary>
        [JsonProperty("FIRST_PRD_AMT")]
        public string FirstPaymentAmount { get; set; }

        /// <summary>
        /// 其餘每期款 期付款
        /// </summary>
        [JsonProperty("OTH_PRD_AMT")]
        public string OtherPaymentAmount { get; set; }

        /// <summary>
        /// 撥款金額
        /// </summary>
        [JsonProperty("DSB_AMT")]
        public string AppropriationAmount { get; set; }

        /// <summary>
        /// 繳款方式：REF: CRS_REF.PAY_TYPE_CD、APP_FORM.PAY_TYPE_CD
        /// </summary>
        [JsonProperty("PAY_TYPE_NME")]
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// 經辦人員
        /// </summary>
        [JsonProperty("VENDER_SALES_NME")]
        public string PersonInCharge { get; set; }

        /// <summary>
        /// 門市
        /// </summary>
        [JsonProperty("VENDER_NME")]
        public string Store { get; set; }

        /// <summary>
        /// 此案件能否留言： 可: Y(廠商APP-拍照:留言拉CASE_DATA_TODO、電子申請單:留言拉到電商進件作業-電商廠商留言回覆, 流程結束也可留)。否: N CASE_SEND_MSG
        /// </summary>
        [JsonProperty("CAN_SEND_MGS")]
        public string IsSendMessage { get; set; }

        /// <summary>
        /// 此案件能否補件：可: Y(廠商APP-拍照、電子申請單:補件只拉電商進件作業流程, 附件也丟到EC, 流程結束也可補)。否: N
        /// </summary>
        [JsonProperty("SUPPLEMENT")]
        public string IsSupplement { get; set; }

        /// <summary>
        /// 此案件能否下載審核通知書：可: Y(進件流程結束)。否: N。DOWNLOAD_CASE_QY_APRVNOTICE_PDF
        /// </summary>
        [JsonProperty("DOWNLOAD_APRVNOTICE")]
        public string IsDownload { get; set; }

        /// <summary>
        /// 此案件能否取消：可: Y。否: N
        /// </summary>
        [JsonProperty("CANCEL")]
        public string IsCancel { get; set; }

        /// <summary>
        /// 此案件能否調降金額/退貨：可: Y。否: N
        /// </summary>
        [JsonProperty("CONTRACT")]
        public string IsAdjustPriceOrReturn { get; set; }

        /// <summary>
        /// 此案件能否試算解約金：可: Y。否: N
        /// </summary>
        [JsonProperty("CNTRT_ABT_CAL")]
        public string IsCalculateCancellationFees { get; set; }

        /// <summary>
        /// 是否顯示匯款帳戶：可: Y。否: N
        /// </summary>
        [JsonProperty("DISPLAY_ACC")]
        public string IsShowAccountNo { get; set; }

        /// <summary>
        /// 送件編號：CASE_ID、EC_CASE_ID、EP_ CASE_ID
        /// </summary>
        [JsonProperty("CASE_ID")]
        public string CaseId { get; set; }

        /// <summary>
        /// VENDER_UPD_RESEND (補件確認)
        /// </summary>
        [JsonProperty("CASE_TODO_TYPE")]
        public string CheckSupplement { get; set; }

        /// <summary>
        /// 匯款銀行名稱
        /// </summary>
        [JsonProperty("BNK_NME")]
        public string BankName { get; set; }

        /// <summary>
        /// 匯款銀行金融機構總代號
        /// </summary>
        [JsonProperty("BNK_ID")]
        public string BankId { get; set; }

        /// <summary>
        /// 匯款銀行金融機構總代號
        /// </summary>
        [JsonProperty("ACCOUNT_NO")]
        public string AccountNo { get; set; }

        /// <summary>
        /// 案件類型
        /// </summary>
        [JsonProperty("APLY_TYPE")]
        public string CaseType { get; set; }

        /// <summary>
        /// 案件留言
        /// </summary>
        [JsonProperty("CASE_RECORD")]
        public List<CaseMessageViewModel> CaseMessage { get; set; }
    }
}