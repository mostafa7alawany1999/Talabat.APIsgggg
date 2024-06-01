using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Models;
using Talabat.Core.Services.Interfaces;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        const string endpointSecret = "whsec_776c8704b6b2ac596d0df6a6d94c775b244e683be620e1f6c731dd6b1d9c023d";

        public PaymentsController(IPaymentService paymentService)
        {
           _paymentService = paymentService;
        }


        #region CreateOrUpdatePayment
        [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost] //Post :/api/Payments
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePayment(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400,"There Is Problem With Your Basket"));

            return Ok(basket);
        }
        #endregion


        #region Webhook EndPoint
        [AllowAnonymous]
        [HttpPost] //Post: https://localhost:44367/api/payments/webhook
        [Route("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                  await  _paymentService.UpdatePaymentIntentToSuccessedOrFailed(paymentIntent.Id, false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentService.UpdatePaymentIntentToSuccessedOrFailed(paymentIntent.Id, true);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
        #endregion




    }
}
