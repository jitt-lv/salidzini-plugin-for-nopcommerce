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
using Nop.Core.Domain.Directory;
using Nop.Services.Directory;
using Nop.Services.Tax;
using System.Globalization;

namespace Nop.Plugin.Feed.Salidzini
{
    public class SalidziniFeedService : ISalidziniFeedService
    {
        private readonly IProductService _productService;
        private readonly ITaxService _taxService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageService _languageService;
        private readonly ICategoryService _categoryService;
        private readonly IPictureService _pictureService;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly CurrencySettings _currencySettings;
        public SalidziniFeedService(
            ICacheManager cacheManager,
            IProductService productService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ILanguageService languageService,
            ICategoryService categoryService,
            IPictureService pictureService,
            ICurrencyService currencyService,
            IPriceCalculationService priceCalculationService,
            ITaxService taxService,
            CurrencySettings currencySettings)
        {
            _workContext = workContext;
            _cacheManager = cacheManager;
            _productService = productService;
            _storeContext = storeContext;
            _categoryService = categoryService;
            _languageService = languageService;
            _pictureService = pictureService;
            _currencyService = currencyService;
            _currencySettings = currencySettings;
            _priceCalculationService = priceCalculationService;
            _taxService = taxService;
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
                    // manufacturer
                    var manufacturer = product.ProductManufacturers
                        .FirstOrDefault()?.Manufacturer?.Name;

                    // name
                    var name = product.GetLocalized(x => x.Name, languageId);
                    if (name.Length > 200) name = name.Substring(0, 200);

                    // model
                    var model = name;

                    // link
                    var link = string.Format("{0}{1}", store.Url, product.GetSeName(languageId));

                    // category_full & category_link
                    // TODO : localize categories              
                    var categoryBreadCrumb = string.Empty;
                    var categoryLink = string.Empty;
                    var defaultProductCategory = _categoryService
                        .GetProductCategoriesByProductId(product.Id, store.Id)
                        .FirstOrDefault();
                    if (defaultProductCategory != null)
                    {
                        categoryBreadCrumb = defaultProductCategory.Category
                             .GetFormattedBreadCrumb(_categoryService, separator: ">", languageId: languageId);

                        categoryLink = string.Format("{0}{1}", store.Url, defaultProductCategory.Category
                            .GetSeName(languageId));
                    }

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

                    // price
                    var currency = _currencyService
                        .GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId);

                    decimal finalPriceBase;
                    // TODO : make configurable
                    if (true)
                    {
                        // calculate for the maximum quantity (in case if we have tier prices)
                        decimal minPossiblePrice = _priceCalculationService
                            .GetFinalPrice(product, _workContext.CurrentCustomer, decimal.Zero, true, int.MaxValue);

                        decimal taxRate;
                        finalPriceBase = _taxService
                            .GetProductPrice(product, minPossiblePrice, out taxRate);
                    }
                    else
                        finalPriceBase = product.Price;

                    decimal price = _currencyService.ConvertFromPrimaryStoreCurrency(finalPriceBase, currency);
                    //round price now so it matches the product details page
                    price = RoundingHelper.RoundPrice(price);


                    results.Add(new SalidziniProductItem
                    {
                        name = name,
                        image = imageUrl,
                        category_full = categoryBreadCrumb,
                        category_link = categoryLink,
                        manufacturer = manufacturer,
                        link = link,
                        model = model,
                        price = price.ToString("0.00", CultureInfo.InvariantCulture),
                        used = "0",
                        in_stock = "1"
                    });
                }
            }

            return
                results;
        }
    }
}
