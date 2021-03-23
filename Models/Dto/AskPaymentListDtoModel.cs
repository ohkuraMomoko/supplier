using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 線上請款列表 DtoModel
    /// </summary>
    public class AskPaymentListDtoModel : BillingSearchBaseDtoModel
    {
        /// <summary>
        /// 交易金額
        /// </summary>
        [JsonProperty("OBJ_TTL_AMT")]
        public int ObjTotalAmount { get; set; }

        /// <summary>
        /// 核准日(YYYYMMDD)
        /// </summary>
        [JsonProperty("CONFIRM_RESERVE_DT")]
        public string ConfirmReserveDt { get; set; }
    }
}