



using System.Web.Mvc;
using System.Web.Routing;

namespace SupplierPlatform
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Grant Query",
                url: "Vender/GrantQuery",
                defaults: new { controller = "Home", action = "GrantQuery" }
            );

            routes.MapRoute(
                name: "Order Detail",
                url: "Vender/OrderDetail",
                defaults: new { controller = "Home", action = "OrderDetail" }
            );

            routes.MapRoute(
                name: "Online Charge",
                url: "Vender/OnlineCharge",
                defaults: new { controller = "Home", action = "OnlineCharge" }
            );

            routes.MapRoute(
               name: "Check Login",
               url: "check_login",
               defaults: new { controller = "Home", action = "CheckLogin" }
            );

            routes.MapRoute(
                name: "Mart",
                url: "onepage/{id}/index",
                defaults: new { controller = "Mart", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Vendor", action = "Index" }
            );
        }
    }
}
