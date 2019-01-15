using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Models.Backend;
using Microsoft.Extensions.Options;
using PayPal.Api;

namespace Backend.Services
{
    public class ServicesMiddleware : ServicesMiddlewareHelpers, IServicesMiddleware
    {
        private readonly ApplicationSettings _applicationSettings;

        public ServicesMiddleware(ApplicationSettings appSettings)
        {
            _applicationSettings = appSettings;
        }

        public ServiceResponse<string> PaymentUrl(PayPalProduct product)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                double cartAmount = 0;
                var itemList = new ItemList();
                var items = new List<Item>();

                var apiContext = GetAPIContext(_applicationSettings.ClientId, _applicationSettings.ClientSecret);

                var payment = new PaypalPayment();
                payment.SiteURL = product.ReturnUrl;
                payment.InvoiceNumber = $"{Guid.NewGuid()}";
                payment.Currency = product.CurrencyCode;
                payment.Tax = $"{product.Tax}";
                payment.ShippingFee = $"{product.ShippingFee}";
                payment.OrderDescription = $"{product.Description}";
                payment.ProductList.Add(product);

                foreach (var cartItem in payment.ProductList)
                {
                    if (cartItem.OrderQty > 0)
                    {
                        var Item = new Item();
                        Item.name = cartItem.Description;
                        Item.currency = payment.Currency;
                        Item.price = Math.Round(cartItem.UnitPrice, 2).ToString();
                        Item.quantity = cartItem.OrderQty.ToString();
                        Item.sku = cartItem.SKU;
                        items.Add(Item);
                        cartAmount += Convert.ToDouble(Math.Round(cartItem.UnitPrice, 2)) * Convert.ToDouble(Item.quantity);
                    }
                }

                itemList.items = items;
                cartAmount = Math.Round(cartAmount, 2);

                var payer = new Payer() { payment_method = "paypal" };
                var redirUrls = new RedirectUrls()
                {
                    cancel_url = payment.SiteURL + "?cancel=true",
                    return_url = payment.SiteURL
                };

                var details = new Details()
                {
                    tax = payment.Tax.ToString(),
                    shipping = payment.ShippingFee.ToString(),
                    subtotal = cartAmount.ToString()
                };

                var paypalAmount = new Amount() { currency = payment.Currency, total = cartAmount.ToString(), details = details };

                var transactionList = new List<Transaction>();
                Transaction transaction = new Transaction();
                transaction.description = payment.OrderDescription;
                transaction.invoice_number = payment.InvoiceNumber;
                transaction.amount = paypalAmount;
                transaction.item_list = itemList;
                transactionList.Add(transaction);

                var processedPayment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };

                var createdPayment = processedPayment.Create(apiContext);
                var links = createdPayment.links.GetEnumerator();
                while (links.MoveNext())
                {
                    var link = links.Current;
                    if (link.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        serviceResponse.Message = "Success";
                        serviceResponse.Success = true;
                        serviceResponse.Response = link.href;
                    }
                }
            }
            catch (Exception error)
            {
                serviceResponse.Message = $"Error while generating payment url, please retry.";
                serviceResponse.Error = error;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public ServiceResponse<Payment> MakePayment(string paymentId, string token, string payerId)
        {
            var serviceResponse = new ServiceResponse<Payment>();
            try
            {
                var apiContext = GetAPIContext(_applicationSettings.ClientId, _applicationSettings.ClientSecret);

                var paymentExecution = new PaymentExecution() { payer_id = payerId };
                var payment = new Payment() { id = paymentId };
                var executedPayment = new Payment();
                executedPayment = payment.Execute(apiContext, paymentExecution);

                if (executedPayment != null && executedPayment.state.ToLower().Equals("approved"))
                {
                    serviceResponse.Message = $"Payment {executedPayment.id} approved.";
                    serviceResponse.Success = true;
                    serviceResponse.Response = executedPayment;
                }
                else
                {
                    serviceResponse.Message = $"Payment {executedPayment.state}.";
                    serviceResponse.Success = false;
                    serviceResponse.Response = executedPayment;
                }
            }
            catch(Exception error)
            {
                serviceResponse.Message = $"Error while making payment, please retry.";
                serviceResponse.Error = error;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }
    }
}
