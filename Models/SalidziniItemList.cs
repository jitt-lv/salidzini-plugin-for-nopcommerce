using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nop.Plugin.Feed.Salidzini.Models
{
    [XmlRoot(ElementName = "root", Namespace = null)]
    public class SalidziniProductList : List<SalidziniProductItem>
    {
    }
    [XmlType(TypeName = "item")]
    public class SalidziniProductItem
    {
        public string name { get; set; }
        public string link { get; set; }
        public string price { get; set; }
        public string image { get; set; }
        public string category_full { get; set; }
        public string category_link { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string in_stock { get; set; }
        public string delivery_cost_riga { get; set; }
        public string delivery_latvija { get; set; }
        public string delivery_latvijas_pasts { get; set; }
        public string delivery_dpd_paku_bode { get; set; }
        public string delivery_pasta_stacija { get; set; }
        public string delivery_omniva { get; set; }
        public string delivery_statoil { get; set; }
        public string delivery_days_riga { get; set; }
        public string delivery_days_latvija { get; set; }
        public string used { get; set; }

    }
}
