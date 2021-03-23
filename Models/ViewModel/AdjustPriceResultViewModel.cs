using Newtonsoft.Json;

namespace SupplierPlatform.Models.ViewModel
{
    public class AdjustPriceResultViewModel : ResultViewModel
    {
        /// <summary>
        /// 解約金(返還金額)
        /// </summary>
        [JsonProperty("ABT_AMT")]
        public int ABT_AMT { get; set; }

        /// <summary>
        /// 手續費
        /// </summary>
        [JsonProperty("HNDL_FEE")]
        public int HNDL_FEE { get; set; }

        /// <summary>
        /// 手續費計算公式文字
        /// </summary>
        [JsonProperty("HNDL_FEE_CAL_FORMULA")]
        public string HNDL_FEE_CAL_FORMULA { get; set; }

        /// <summary>
        /// 解約金計算公式文字
        /// </summary>
        [JsonProperty("ABT_AMT_CAL_FORMULA")]
        public string ABT_AMT_CAL_FORMULA { get; set; }
    }
}