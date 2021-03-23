using System.Collections.Generic;
using static SupplierPlatform.Content.CaseController;

namespace SupplierPlatform.Models.ViewModel
{
    public class OrderInquiryParamsViewModel
    {
        /// <summary>
        /// 供應商帳號MEMBER_ID
        /// </summary>
        public int MEMBER_ID { get; set; }

        /// <summary>
        /// 分頁指標
        /// </summary>
        public int INDEX { get; set; }

        /// <summary>
        /// 每頁筆數標
        /// </summary>
        public int PAGE_NUM { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string ORDER_ID { get; set; }

        /// <summary>
        /// 訂單建立近期幾天
        /// </summary>
        public int? ORDER_DAYS { get; set; }

        /// <summary>
        /// 訂單建立起日
        /// </summary>
        public string ORDER_DT_START { get; set; }

        /// <summary>
        /// 訂單建立迄日
        /// </summary>
        public string ORDER_DT_END { get; set; }

        public List<OrderInquiryParamsStatus> STATUS { get; set; }

        /// <summary>
        /// 底層供應商ID
        /// </summary>
        public string VENDER_ID { get; set; }

        /// <summary>
        /// 消費者ID
        /// </summary>
        public string CUST_ID { get; set; }

        /// <summary>
        /// 消費者姓名
        /// </summary>
        public string CUST_NME { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string APP_NO { get; set; }

        /// <summary>
        /// 車牌號碼
        /// </summary>
        public string LINC_NO { get; set; }

        /// <summary>
        /// 是否為一頁式電商
        /// </summary>
        public string IS_EC_QY { get; set; }
    }
}