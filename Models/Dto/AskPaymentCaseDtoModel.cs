using Newtonsoft.Json;

namespace SupplierPlatform.Models.Dto
{
    /// <summary>
    /// 線上請款 案件明細
    /// </summary>
    public class AskPaymentCaseDtoModel: CaseBaseDtoModel
    {
        /// <summary>
        /// 來源站台
        /// </summary>
        [JsonProperty("CHANNEL")]
        public string Channel { get; set; }

        /// <summary>
        /// 撥款金額
        /// </summary>
        [JsonProperty("DSB_AMT")]
        public string AppropriationAmount { get; set; }

        /// <summary>
        /// 是否可取消案件：可取消:Y 不可取消:N
        /// </summary>
        [JsonProperty("IS_CAN_CANCEL")]
        public string IsCanCancel { get; set; }
    }
}