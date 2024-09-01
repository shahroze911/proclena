using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proclena_backend.Models
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }

        public byte[] ExistingProductImage { get; set; }

        // This will be used to collect dynamic properties
        public List<PropertyViewModel> ProductProperties { get; set; } = new List<PropertyViewModel>();
    }

    public class PropertyViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}