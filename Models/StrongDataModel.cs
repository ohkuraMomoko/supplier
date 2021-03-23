using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplierPlatform.Models
{
    /// <summary>
    /// 據點資料
    /// </summary>
    public class StrongDataModel: StoreDataModel
    {
        /// <summary>
        /// 門市id
        /// </summary>
        [JsonProperty("storeid")]
        public string Storeid { get; set; }

    }
}