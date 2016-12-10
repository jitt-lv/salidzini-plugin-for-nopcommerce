using Nop.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Feed.Salidzini
{
    public class SalidziniFeedService : ISalidziniFeedService
    {
        ICacheManager _cacheManager;
        public SalidziniFeedService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public string GetXMLProductFeed()
        {
            return "xml goes here";
        }
    }
}
