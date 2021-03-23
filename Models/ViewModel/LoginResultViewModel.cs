using Newtonsoft.Json;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    public class LoginResultViewModel: UserInfoBaseModel
    {
        /// <summary>
        /// 查詢狀態：0正常 -1系統發生錯誤
        /// </summary>
        [JsonProperty("RTN_CD")]
        public int RtnCd { get; set; }

        /// <summary>
        /// 提示訊息
        /// </summary>
        [JsonProperty("ALERT_MSG")]
        public string AlertMsg { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [JsonProperty("VEND_SALE_NME")]
        public string VendSaleName { get; set; }

        /// <summary>
        /// 門市代碼
        /// </summary>
        [JsonProperty("VNO2")]
        public string Vno2 { get; set; }

        /// <summary>
        /// 是否有同意會員條款 1.是, 2否
        /// </summary>
        [JsonProperty("APPROVE_POLICY")]
        public string ApprovePolicy { get; set; }

        /// <summary>
        /// 是否為中租零卡支付供應商 1.是, 2否
        /// </summary>
        [JsonProperty("IS_EP_VENDER")]
        public string IsEpVender { get; set; }

        /// <summary>
        /// 是否有零利率報價1.是, 2否
        /// </summary>
        [JsonProperty("EP_RATE_TYPE_1")]
        public string EpRateType1 { get; set; }

        /// <summary>
        /// 是否有低利率報價1.是, 2否
        /// </summary>
        [JsonProperty("EP_RATE_TYPE_2")]
        public string EpRateType2 { get; set; }

        /// <summary>
        /// 預設產品名稱
        /// </summary>
        [JsonProperty("DEFAULT_PROD_NME")]
        public string DefaultProdName { get; set; }

        /// <summary>
        /// 門市狀態 往來狀態: REF:CRS_REF.VENDER_STATUS_CD A.正常 S.停用 P.靜止戶 L.缺件
        /// </summary>
        [JsonProperty("VENDER_STATUS_CD")]
        public string VenderStatusCd { get; set; }

        /// <summary>
        /// 是否有主管權限 1:是 2:否
        /// </summary>
        [JsonProperty("IS_ROLE01_TYPE")]
        public string IsRole01Type { get; set; }

        /// <summary>
        /// 是否有請款權限 1:是 2:否
        /// </summary>
        [JsonProperty("IS_ROLE02_TYPE")]
        public string IsRole02Type { get; set; }

        /// <summary>
        /// 一頁式電商的單筆最低交易金額(元)
        /// (OBJ_TTL_AMT標的物金額必須大於每單最低交易金額)
        /// </summary>
        [JsonProperty("EP_LST_ONCE_AMT")]
        public int EpLstOnceAmt { get; set; }

        /// <summary>
        /// 供應商平台的單筆最低交易金額(元)
        /// (OBJ_TTL_AMT標的物金額必須大於每單最低交易金額)
        /// </summary>
        [JsonProperty("AR_LST_ONCE_AMT")]
        public int ArLstOnceAmt { get; set; }

        /// <summary>
        /// AR單筆最低期付款(元)
        /// 一頁式電商案件屬零卡案不用控最低期付款
        /// </summary>
        [JsonProperty("AR_LST_PRD_AMT")]
        public int ArLstPrdAmt { get; set; }

        /// <summary>
        /// 功能清單
        /// </summary>
        [JsonProperty("MENU_LIST")]
        public List<MenuViewModel> Mnmus { get; set; }
    }
}