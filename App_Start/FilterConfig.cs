using SupplierPlatform.Filter;
using SupplierPlatform.Services;
using System.Web;
using System.Web.Mvc;

namespace SupplierPlatform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogRequestFilter());
            filters.Add(new AuthFilter(new OperatorSessionContext()));
            filters.Add(new HandleErrorAttribute());
        }
    }
}
