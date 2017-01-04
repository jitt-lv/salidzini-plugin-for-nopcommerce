using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Plugins;
using Nop.Plugin.Feed.Salidzini.Models;
using Nop.Services.Catalog;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Feed.Salidzini.Controllers
{
    public class SalidziniFeedController : BasePluginController
    {

        private readonly ISalidziniFeedService _salidziniFeedService;
        public SalidziniFeedController(ISalidziniFeedService salidziniFeedService)
        {
            _salidziniFeedService = salidziniFeedService;
        }

        [ChildActionOnly]
        public ActionResult Configure()
        {
            // TODO : Return default settings
            var defaultConfig = new ConfigurationModel();

            return 
                View("~/Plugins/Feed.Salidzini/Views/SalidziniFeed/Configure.cshtml", defaultConfig);
        }
        [HttpPost]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            // TODO : Implement any necessary settings update
            if (!ModelState.IsValid)
            {
                return Configure();
            }

            return Configure();
        }

        [HttpGet]
        public ActionResult Get()
        {
            var productFeed = _salidziniFeedService
                .GetProductsFeed();

            // TODO : Consider some caching

            return 
                new XmlResult<SalidziniProductList>(productFeed);
        }

        [ChildActionOnly]
        public ActionResult ShowLogo(string widgetZone, object additionalData = null)
        {
            // TODO : Return default settings
            var logosModel = new LogosModel();

            return
                View("~/Plugins/Feed.Salidzini/Views/SalidziniFeed/Logos.cshtml", logosModel);
        }
    }
}
