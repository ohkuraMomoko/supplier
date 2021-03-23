using Newtonsoft.Json;
using SupplierPlatform.Models.Dto;
using System.Collections.Generic;

namespace SupplierPlatform.Models
{
    /// <summary>
    /// 最新案件
    /// </summary>
    public class CaseInquiryViewModel
    {
        /// <summary>
        /// 案件查詢參數
        /// </summary>
        public CaseParamsModel datas { get; set; }
    }


    public class CaseParamsModel
    {
        /// <summary>
        /// 搜尋類型 1: 訂單編號 2:身分證字號 3:姓名 4:案件編號 5:車牌號碼
        /// </summary>
        [JsonProperty("searchType")]
        public int? SearchType { get; set; }
        /// <summary>
        /// 搜尋內容
        /// </summary>
        [JsonProperty("search")]
        public string Search { get; set; }
        /// <summary>
        ///  選擇日期類型 1: 近期幾天 2: 某段時間
        /// </summary>
        [JsonProperty("dateType")]
        public int? DateType { get; set; }
        /// <summary>
        /// 近期幾天 1:當日 2:近3天 3:近30天 4:近60天
        /// </summary>
        [JsonProperty("rangeDate")]
        public int? RangeDate { get; set; }
        /// <summary>
        /// 選擇開始日期 起訖日填寫 限定過去60天內
        /// </summary>
        [JsonProperty("startDate")]
        public string StartDate { get; set; }
        /// <summary>
        /// 選擇結束日期 起訖日填寫 限定過去60天內
        /// </summary>
        [JsonProperty("endDate")]
        public string EndDate { get; set; }
        /// <summary>
        /// 狀態 1: 尚未操作 2:審核中 3:婉拒(未通過) 4: 取消案件5:已核准未請款 6:請款中 7:已撥款  8: 已核准處理中9:退貨 10:調降金額處理中 
        /// </summary>
        [JsonProperty("status")]
        public List<int> Status { get; set; }
        /// <summary>
        /// 門市 僅有總公司帳號權限者有此欄位
        /// </summary>
        [JsonProperty("store")]
        public string Store { get; set; }
        /// <summary>
        /// 分頁位置
        /// </summary>
        [JsonProperty("pageNo")]
        public int PageNo { get; set; } = 0;
        /// <summary>
        /// 每頁最多筆數
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; } = 20;
    }
}