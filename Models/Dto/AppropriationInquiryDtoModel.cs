using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 查詢撥款 DtoModel
    /// </summary>
    public class AppropriationInquiryDtoModel : BillingSearchBaseDtoModel
    {
        /// <summary>
        /// 撥款日近期幾天 (當天1、近三天 3 )
        /// </summary>
        [JsonProperty("DAYS")]
        public string Days { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        [JsonProperty("CUST_ID")]
        public string CustId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("CUST_NME")]
        public string CustName { get; set; }

        /// <summary>
        /// 車牌號碼
        /// </summary>
        [JsonProperty("LINC_NO")]
        public string LincNo { get; set; }
    }
}