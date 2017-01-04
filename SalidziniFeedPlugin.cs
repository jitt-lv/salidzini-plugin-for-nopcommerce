using Nop.Core.Plugins;
using System;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Shipping;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Shipping.Pickup;
using Nop.Services.Shipping.Tracking;
using Nop.Web.Framework.Menu;
using System.Linq;
using Nop.Services.Cms;
using System.Collections.Generic;

namespace Nop.Plugin.Feed.Salidzini
{
    public class SalidziniFeedPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin // , IAdminMenuPlugin
    {
        private readonly TrackingRecordObjectContext _context;

        public SalidziniFeedPlugin(TrackingRecordObjectContext context)
        {
            _context = context;
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "SalidziniFeed";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Feed.Salidzini.Controllers" }, { "area", null } };
        }

        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "ShowLogo";
            controllerName = "SalidziniFeed";
            routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Feed.Salidzini.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string> { "footer" };
        }

        public override void Install()
        {
            _context.Install();
            // this.AddOrUpdatePluginLocaleResource("Plugins.Pickup.PickupInStore.AddNew", "Add a new pickup point");

            base.Install();
        }
        /*
        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "YourCustomSystemName",
                Title = "Plugin Title",
                ControllerName = "ControllerName",
                ActionName = "List",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }
        */
        public override void Uninstall()
        {
            _context.Uninstall();
            // this.DeletePluginLocaleResource("Plugins.Pickup.PickupInStore.AddNew");
            base.Uninstall();
        }
    }
}
