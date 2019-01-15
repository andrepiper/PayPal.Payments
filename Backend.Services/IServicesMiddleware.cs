using Common.Models.Backend;
using PayPal.Api;
using System;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IServicesMiddleware
    {
        ServiceResponse<string> PaymentUrl(PayPalProduct product);
        ServiceResponse<Payment> MakePayment(string paymentId, string token, string payerId);
    }
}
