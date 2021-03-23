using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplierPlatform.Models.ViewModel
{
    /// <summary>
    /// 撥款概況
    /// </summary>
    public class AppropriationOverviewViewModel
    {
        /// <summary>
        /// 撥款日期
        /// </summary>
        [JsonProperty("appropriation_date")]
        public string AppropriationDate { get; set; }

        /// <summary>
        /// 筆數
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>
        /// 撥款金額
        /// </summary>
        [JsonProperty("appropriation_price")]
        public string AppropriationPrice { get; set; }
    }
}