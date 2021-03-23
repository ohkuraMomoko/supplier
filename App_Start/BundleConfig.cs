using System.Web;
using System.Web.Optimization;

namespace SupplierPlatform
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/layout.min.css"));
            //bundles.Add(new ScriptBundle("~/bundles/vue").Include(
            //            "~/Scripts/vue.js"));
        }
    }
}
