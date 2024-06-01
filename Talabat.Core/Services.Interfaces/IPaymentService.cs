using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order;

namespace Talabat.Core.Services.Interfaces
{
    public interface IPaymentService
    {

        Task<CustomerBasket>CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdatePaymentIntentToSuccessedOrFailed(string paymentIntentId, bool flag);


    }
}
