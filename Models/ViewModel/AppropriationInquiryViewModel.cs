using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 查詢撥款 View Model
    /// </summary>
    public class AppropriationInquiryViewModel : SearchBaseViewModel
    {
        /// <summary>
        /// 選擇日期類型：1: 近期幾天 2: 某段時間
        /// </summary>
        [JsonProperty("date_type")]
        public int DateType { get; set; }

        /// <summary>
        /// 近期幾天：1:當日 2:近3天 3:近30天 4:近60天
        /// </summary>
        [JsonProperty("ORDER_DAYS")]
        public int RangeDate { get; set; }

        /// <summary>
        /// 選擇開始日期：起訖日填寫 限定過去60天內
        /// </summary>
        [JsonProperty("ORDER_DT_START")]
        public string StartDt { get; set; }

        /// <summary>
        /// 選擇結束日期：起訖日填寫 限定過去60天內
        /// </summary>
        [JsonProperty("ORDER_DT_END")]
        public string EndDt { get; set; }
    }
}