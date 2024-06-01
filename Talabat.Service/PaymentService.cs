using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Interfaces;
using Talabat.Core.Specifications.OrderSpec;
using Product = Talabat.Core.Models.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepository basketRepository , IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {

            #region 1.Get Basket
           
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) return null;

            #endregion

            #region 2. Total Price( Basket Not Null )

            
            if (basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }
                // Subtotal
                var subTotal = basket.Items.Sum(I=>I.Price * I.Quantity);

                //Cost DeliveryMethod
                 var ShippingPrice = 0m;
                if(basket.DeliveryMethodId.HasValue)
                {
                  var deliveryMethod =  await  _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                    ShippingPrice = deliveryMethod.Cost;  
                }

            #endregion

            #region 3. Call Stripe

            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create New PaymentIntentId
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long) (subTotal * 100 + ShippingPrice * 100),
                    Currency ="usd",
                    PaymentMethodTypes = new List<string>() { "card"},

                };
               paymentIntent = await service.CreateAsync(Options);
               basket.PaymentIntentId = paymentIntent.Id;
               basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // Update PaymentIntentId

                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + ShippingPrice * 100),
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, Options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            #endregion

            #region 4. Return Basket Included PaymentIntentId And Client Client Secret
            await _basketRepository.UpdateBasketAsync(basket);
             return basket;
            #endregion
        }

        public async Task<Order> UpdatePaymentIntentToSuccessedOrFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetwithSpecAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
