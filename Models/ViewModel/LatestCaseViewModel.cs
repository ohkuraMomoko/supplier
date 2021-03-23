using SupplierPlatform.Models.Dto;
using System.Collections.Generic;

namespace SupplierPlatform.Models
{
    /// <summary>
    /// 最新案件
    /// </summary>
    public class LatestCaseViewModel : ResultViewModel
    {
        /// <summary>
        /// 案件資料清單
        /// </summary>
        public List<CaseDtoModel> CASE_LIST { get; set; }
    }
}