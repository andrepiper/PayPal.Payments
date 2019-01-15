using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models.Backend
{
    public class PayPalProduct
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public string SKU { get; set; }
        public int OrderQty { get; set; }
        public string CurrencyCode { get; set; }
        public double Tax { get; set; }
        public double ShippingFee { get; set; }
        public string ReturnUrl { get; set; }
    }
}
