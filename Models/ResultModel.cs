namespace SupplierPlatform.Models
{
    /// <summary>
    /// 基底 API 執行狀態及訊息
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 數字型態0代表成功; -1代表異常。
        /// </summary>
        public int ReturnCode { get; set; }

        /// <summary>
        /// 系統使用之訊息，不顯示於前端。
        /// </summary>
        public string ReturnMsg { get; set; }

        /// <summary>
        /// 顯示於前端的訊息。
        /// </summary>
        public string Alert { get; set; }

        /// <summary>
        /// 顯示Data內容筆數。
        /// </summary>
        public int Count { get; set; }
    }
}