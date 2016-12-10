using Nop.Web.Framework.Mvc.Routes;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Feed.Salidzini
{
    public class SalidziniFeedRouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Plugin.Feed.Salidzini.Get",
                "Plugins/SalidziniFeed/Get",
                new
                {
                    controller = "SalidziniFeed",
                    action = "Get"
                }, new[] {
                    "Nop.Plugin.Feed.Salidzini.Controllers" });
        }
        public int Priority
        {
            get { return 0; }
        }
    }
}
