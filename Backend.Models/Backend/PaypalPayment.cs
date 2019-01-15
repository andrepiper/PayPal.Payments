using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models.Backend
{
    public class PaypalPayment
    {
        public PaypalPayment()
        {
            ProductList = new List<PayPalProduct>();
        }

        public List<PayPalProduct> ProductList { get; set; }
        public string SiteURL { get; set; }
        public string InvoiceNumber { get; set; }
        public string Currency { get; set; }
        public string Tax { get; set; }
        public string ShippingFee { get; set; }
        public string OrderDescription { get; set; }

        public string PayerID { get; set; }
        public string PaymentID { get; set; }
        public string Token { get; set; }

        public int? Count { get; set; }
        public string StartID { get; set; }
        public int? StartIndex { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartDate { get; set; }
        public string PayeeEmail { get; set; }
        public string PayeeID { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
    }
}
