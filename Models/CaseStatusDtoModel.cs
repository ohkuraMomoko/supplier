using Newtonsoft.Json;

namespace SupplierPlatform.Models
{
    /// <summary>
    /// 案件狀態
    /// </summary>
    public class CaseStatusDtoModel
    {
        /// <summary>
        /// 狀態描述
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 狀態類型：1: 尚未操作 2:審核中 3:婉拒(未通過) 4: 取消案件 5:已核准未請款 6:請款中 7:已撥款  8: 已核准處理中 9:退貨 10:調降金額處理中
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }
    }
}