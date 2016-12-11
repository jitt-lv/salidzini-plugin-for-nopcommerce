using Nop.Core.Domain.Stores;
using Nop.Plugin.Feed.Salidzini.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Feed.Salidzini
{
    public interface ISalidziniFeedService
    {
        SalidziniProductList GetProductsFeed();
    }
}
