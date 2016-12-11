﻿using Nop.Core;
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
        private readonly IProductService _productService;
        private readonly ISalidziniFeedService _salidziniFeedService;
        private readonly IWorkContext _workContext;

        public SalidziniFeedController(IWorkContext workContext,
            ISalidziniFeedService salidziniFeedService,
            IProductService productService,
            IPluginFinder pluginFinder)
        {
            _workContext = workContext;
            _salidziniFeedService = salidziniFeedService;
            _productService = productService;
        }

        [ChildActionOnly]
        public ActionResult Configure()
        {
            var defaultConfig = new ConfigurationModel();
            return View("~/Plugins/Feed.Salidzini/Views/SalidziniFeed/Configure.cshtml", defaultConfig);
        }
        [HttpPost]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
            {
                return Configure();
            }

            return Configure();
        }

        [HttpGet]
        public ActionResult Get()
        {
            //Read from the product service
            Product productById = _productService.GetProductById(1);
            /*
            //If the product exists we will log it
            if (productById != null)
            {
                //Setup the product to save
                var record = new TrackingRecord();
                record.ProductId = productId;
                record.ProductName = productById.Name;
                record.CustomerId = _workContext.CurrentCustomer.Id;
                record.IpAddress = _workContext.CurrentCustomer.LastIpAddress;
                record.IsRegistered = _workContext.CurrentCustomer.IsRegistered();

                //Map the values we're interested in to our new entity
                _viewTrackingService.Log(record);
            }
            */

            //Return the view, it doesn't need a model
            var list = new SalidziniItemList { new SalidziniProductItem { name = "Nokia" } };
            return new XmlResult<SalidziniItemList>(list);
        }
    }
}
