using Talabat.Core.Models.Order;

namespace Talabat.APIs.DTO
{
    public class OrderToReturnDto
    {

        public int Id { get; set; } 
        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public OrderStatus Status { get; set; } 

        public Address ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();

        public decimal SubTotal { get; set; }

        public string PaymentIntentId { get; set; } = string.Empty;


    }
}
