using Newtonsoft.Json;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 案件明細 ViewModel
    /// </summary>
    public class CaseGetCurrentDetailViewModel
    {
        public List<CaseCurrentDetail> datas { get; set; }
    }

    public class CaseCurrentDetail
    {
        /// <summary>
        /// 申請日期
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }
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
        /// 狀態
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }
        /// <summary>
        /// 申請人
        /// </summary>
        [JsonProperty("applicant")]
        public string Applicant { get; set; }
        /// <summary>
        /// 交易金額
        /// </summary>
        [JsonProperty("price")]
        public string Price { get; set; }
        /// <summary>
        /// 期數
        /// </summary>
        [JsonProperty("period")]
        public string Period { get; set; }
        /// <summary>
        /// 案件類型
        /// </summary>
        [JsonProperty("casetype")]
        public int CaseType { get; set; }
    }

}