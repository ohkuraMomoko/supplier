using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 訂單/案件相關 viewModel
    /// </summary>
    public class OrderInquiryViewModel : AppropriationInquiryViewModel
    {
        /// <summary>
        /// 案件狀態
        /// </summary>
        [JsonProperty("status")]
        public CaseStatusDtoModel Status { get; set; }

        /// <summary>
        /// 門市：僅有總公司帳號權限者有此欄位
        /// </summary>
        [JsonProperty("store")]
        public string Store { get; set; }

        /// <summary>
        /// 分頁位置：預設為0
        /// </summary>
        [JsonProperty("INDEX")]
        public int PageNo { get; set; }

        /// <summary>
        /// 每頁最多筆數：預設為10
        /// </summary>
        [JsonProperty("PAGE_NUM")]
        public int PageSize { get; set; }








    }
}