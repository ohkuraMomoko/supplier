using Newtonsoft.Json;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 查詢撥款 Result ViewModel
    /// </summary>
    public class BillingAppropriationInquiryResultViewModel
    {
        /// <summary>
        /// 查詢撥款
        /// </summary>
        public List<AppropriationInquiryResultViewModel> Data { get; set; }

        /// <summary>
        /// 撥款概況結果
        /// </summary>
        public List<AppropriationDataModel> Overview { get; set; }

        /// <summary>
        /// 基底 API 執行狀態及訊息
        /// </summary>
        public ResultModel Result { get; set; }
    }

    /// <summary>
    /// 撥款明細匯出參數
    /// </summary>
    public class BillingCurrentDetailViewModel
    {
        public List<BillingCurrentDetail> datas { get; set; }
    }

    public class BillingCurrentDetail
    {
        /// <summary>
        /// 撥款日期
        /// </summary>
        [JsonProperty("appropriationdate")]
        public string AppropriationDate { get; set; }
        /// <summary>
        /// 門市
        /// </summary>
        [JsonProperty("store")]
        public string Store { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        [JsonProperty("ordersn")]
        public string OrderSN { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        [JsonProperty("casesn")]
        public string CaseSN { get; set; }
        /// <summary>
        /// 申請人
        /// </summary>
        [JsonProperty("applicant")]
        public string Applicant { get; set; }
        /// <summary>
        /// 期數
        /// </summary>
        [JsonProperty("period")]
        public string Period { get; set; }
        /// <summary>
        /// 交易金額
        /// </summary>
        [JsonProperty("price")]
        public string Price { get; set; }
    }
}