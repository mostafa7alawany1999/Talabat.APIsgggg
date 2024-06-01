using AutoMapper;
using Microsoft.Extensions.Configuration;
using Talabat.APIs.DTO;
using Talabat.Core.Models;
using Talabat.Core.Models.Order;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.ProductUrl))
            {
                return $"{_configuration["BaseUrl"]}/{source.Product.ProductUrl}";
            }
            return string.Empty;
        }
    }
}
