using System.Web.Mvc;

namespace SupplierPlatform.Services.Interface
{
    public interface IExeclService
    {
        /// <summary>
        /// 匯出當前明細 excel
        /// </summary>
        /// <returns></returns>
        FileStreamResult GetCurrentDetail(string[] rowTitle, string[][] rowData);
    }
}