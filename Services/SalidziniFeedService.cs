using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Stores;
using Nop.Plugin.Feed.Salidzini.Models;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Seo;
using Nop.Services.Media;

namespace Nop.Plugin.Feed.Salidzini
{
    public class SalidziniFeedService : ISalidziniFeedService
    {
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageService _languageService;
        private readonly ICategoryService _categoryService;
        private readonly IPictureService _pictureService;
        public SalidziniFeedService(
            ICacheManager cacheManager,
            IProductService productService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ILanguageService languageService,
            ICategoryService categoryService,
            IPictureService pictureService)
        {
            _workContext = workContext;
            _cacheManager = cacheManager;
            _productService = productService;
            _storeContext = storeContext;
            _categoryService = categoryService;
            _languageService = languageService;
            _pictureService = pictureService;
        }
        public SalidziniProductList GetProductsFeed()
        {
            var results = new SalidziniProductList();

            //Read from the product service
            var store = _storeContext.CurrentStore;

            //language
            var languageId = 0;
            var languages = _languageService.GetAllLanguages(storeId: store.Id);
            //if we have only one language, let's use it
            if (languages.Count == 1)
            {
                //let's use the first one
                var language = languages.FirstOrDefault();
                languageId = language != null ? language.Id : 0;
            }
            //otherwise, use the current one
            if (languageId == 0)
                languageId = _workContext.WorkingLanguage.Id;

            var products1 = _productService.SearchProducts(storeId: store.Id, visibleIndividuallyOnly: true);
            foreach (var product1 in products1)
            {
                var productsToProcess = new List<Product>();
                switch (product1.ProductType)
                {
                    case ProductType.SimpleProduct:
                        {
                            //simple product doesn't have child products
                            productsToProcess.Add(product1);
                        }
                        break;
                    case ProductType.GroupedProduct:
                        {
                            //grouped products could have several child products
                            var associatedProducts = _productService.GetAssociatedProducts(product1.Id, store.Id);
                            productsToProcess.AddRange(associatedProducts);
                        }
                        break;
                    default:
                        continue;
                }

                foreach (var product in productsToProcess)
                {
                    // name
                    var name = product.GetLocalized(x => x.Name, languageId);
                    if (name.Length > 200) name = name.Substring(0, 200);

                    // link
                    var link = string.Format("{0}{1}", store.Url, product.GetSeName(languageId));

                    // category_full
                    // TODO : localize categories              
                    string category = string.Empty;
                    var defaultProductCategory = _categoryService
                        .GetProductCategoriesByProductId(product.Id, store.Id)
                        .FirstOrDefault();
                    if (defaultProductCategory != null)
                        category = defaultProductCategory.Category
                            .GetFormattedBreadCrumb(_categoryService, separator: ">", languageId: languageId);

                    // manufacturer
                    var manufacturer = product.ProductManufacturers
                        .FirstOrDefault()?.Manufacturer?.Name;

                    // image
                    string imageUrl = null;
                    var picture = _pictureService
                        .GetPicturesByProductId(product.Id, 1)
                            ?.FirstOrDefault();
                    if (picture != null)
                        imageUrl = _pictureService
                            .GetPictureUrl(picture, 180, storeLocation: store.Url);
                    // TODO : make configurable (use nop default or not)
                    /*
                    else
                        imageUrl = _pictureService
                            .GetDefaultPictureUrl(180, storeLocation: store.Url);
                    */
                    results.Add(new SalidziniProductItem
                    {
                        name = name,
                        image = imageUrl,
                        category_full = category,
                        manufacturer = manufacturer
                    });
                }
            }

            return
                results;
        }
    }
}
