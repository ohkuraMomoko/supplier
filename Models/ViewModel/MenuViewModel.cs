using Newtonsoft.Json;
using System.Collections.Generic;

namespace SupplierPlatform.Models.ViewModel
{
    public class MenuViewModel
    {
        /// <summary>
        /// 可用功能ID
        /// </summary>
        [JsonProperty("FUNC_ID")]
        public string FuncId { get; set; }

        /// <summary>
        /// 可用功能名稱
        /// </summary>
        [JsonProperty("FUNC_NAME")]
        public string FuncName { get; set; }
    }
}