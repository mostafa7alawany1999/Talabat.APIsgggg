using AutoMapper;
using Talabat.APIs.DTO;
using Talabat.Core.Models;
using UserAddress  = Talabat.Core.Models.Identity.Address;
using OrderAddress = Talabat.Core.Models.Order.Address;
using Talabat.Core.Models.Order;

namespace Talabat.APIs.Helpers
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Product,ProductToReturnDto>().ForMember(D => D.Brand ,    O => O.MapFrom(S => S.Brand.Name))
                                                   .ForMember(D => D.Category,  O => O.MapFrom(S => S.Category.Name))
                                                   .ForMember(D => D.PictureUrl,O => O.MapFrom<ProductPictureUrlResolver>());


            CreateMap<UserAddress, AddressDto>().ReverseMap();
            CreateMap<OrderAddress, AddressDto>().ReverseMap()
                                                 .ForMember(d=>d.FirstName,O=>O.MapFrom(S=>S.FName))
                                                 .ForMember(d => d.LastName, O => O.MapFrom(S => S.LName));






            CreateMap<Order, OrderToReturnDto>()
                     .ForMember(d=>d.DeliveryMethod, O=> O.MapFrom(S=> S.DeliveryMethod.ShortName))
                     .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
             .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
             .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
             .ForMember(d => d.ProductUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());



            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
        }                                          
    }
}
