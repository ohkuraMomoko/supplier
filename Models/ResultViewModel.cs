namespace SupplierPlatform.Models
{
    /// <summary>
    /// Result ViewModel
    /// </summary>
    public class ResultViewModel
    {
        /// <summary>
        /// 執行狀態：0正常 -1錯誤
        /// </summary>
        public int RTN_CD { get; set; }

        /// <summary>
        /// 提示訊息
        /// </summary>
        public string ALERT_MSG { get; set; }
    }
}