using Newtonsoft.Json;
using SupplierPlatform.Enums;
using SupplierPlatform.Helps;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 通知訊息 ViewModel
    /// </summary>
    public class NotificationResultViewModel
    {
        /// <summary>
        /// 訊息id
        /// </summary>
        [JsonProperty("MAJ_SEQ_ID")]
        public string Id { get; set; }

        /// <summary>
        /// 1. 一般案件, 2. 廣告類, 4.中租零卡支付案件
        /// </summary>
        [JsonProperty("NOTI_TYPE")]
        public string TitleType { get; set; }

        /// <summary>
        /// 對應標題類型
        /// </summary>
        public string Title
        {
            get => EnumAttributeHelper.GetEnumDescription((TitleTypeEnum)(int.Parse(this.TitleType)));
        }

        /// <summary>
        /// 案件記錄MAJ_SEQ_ID (for一般案件)
        /// </summary>
        [JsonProperty("NOTI_CASE_RECORD_ID")]
        public string NOTI_CASE_RECORD_ID { get; set; }

        /// <summary>
        /// 案件CASE_ID
        /// </summary>
        [JsonProperty("NOTI_CASE_ID")]
        public string NOTI_CASE_ID { get; set; }

        /// <summary>
        /// 推播MAJ_SEQ_ID
        /// </summary>
        [JsonProperty("MSG_MAJ")]
        public string MSG_MAJ { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        [JsonProperty("NOTI_CNT")]
        public string Content { get; set; }

        /// <summary>
        /// 訊息時間
        /// </summary>
        [JsonProperty("NOTI_DT")]
        public string Date { get; set; }

        /// <summary>
        /// 訊息是否讀過
        /// </summary>
        [JsonProperty("IS_READ")]
        public string IS_READ { get; set; }

        /// <summary>
        /// 訊息是否讀過
        /// </summary>
        public bool IsReady
        {
            get => this.IS_READ == "1";
        }

        /// <summary>
        /// Web Url
        /// </summary>
        [JsonProperty("WEBURL")]
        public string WebUrl { get; set; }
    }
}