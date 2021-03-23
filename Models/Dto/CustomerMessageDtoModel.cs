using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 客服留言資料
    /// </summary>
    public class CustomerMessageDtoModel
    {
        /// <summary>
        /// 廠商會員發的訊息Y ;客服發的訊息N
        /// </summary>
        [JsonProperty("IS_MEMBER_MSG")]
        public string Who { get; set; }

        /// <summary>
        /// 回覆訊息
        /// </summary>
        [JsonProperty("REPLY_MSG")]
        public string Reply { get; set; }

        /// <summary>
        /// 留言時間：格式 YYYY/MM/DD HH:mm
        /// </summary>
        [JsonProperty("MSG_DT")]
        public string ReplyDt { get; set; }

        /// <summary>
        /// 廠商會員留言最大序號
        /// </summary>
        [JsonProperty("MAX_MAJ_SEQ_ID")]
        public int Sort { get; set; }

        /// <summary>
        /// 廠商會員是否已讀客服回覆訊息Y/N
        /// </summary>
        [JsonProperty("IS_READ")]
        public string IsRead { get; set; }
    }
}