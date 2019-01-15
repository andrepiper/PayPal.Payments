using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Services;
using Common.Models.Backend;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;

namespace Backend.Api.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IServicesMiddleware _servicesMiddlware;

        public PaymentsController(IServicesMiddleware servicesMiddlware)
        {
            _servicesMiddlware = servicesMiddlware;
        }
        [HttpGet]
        public ServiceResponse<string> Info()
        {
            return new ServiceResponse<string>()
            {
                Message = "Sample Json Response from API",
                Response = "Common.Models.Backend.ServiceResponse<T>{ Exception Error, String Messsage, Bool Success, T Response}",
                Success = true
            };
        }

        [Route("paymenturl")]
        [HttpPost]
        public ServiceResponse<string> PaymentUrl(PayPalProduct product)
        {
            var response =_servicesMiddlware.PaymentUrl(product);
            return response;
        }
        [Route("payment")]
        [HttpGet]
        public ServiceResponse<Payment> Payment(string paymentId, string token, string payerId)
        {
            var response = _servicesMiddlware.MakePayment(paymentId, token, payerId);
            return response;
        }
    }
}
