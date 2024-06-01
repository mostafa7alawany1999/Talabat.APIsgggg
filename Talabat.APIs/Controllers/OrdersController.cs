using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.Core.Models.Order;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Interfaces;

namespace Talabat.APIs.Controllers
{

    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController( IOrderService orderService , IMapper mapper , IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #region  CreateOrder
        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost] // Post : api/Orders
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto model)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<AddressDto, Address>(model.ShippingAddress);
            var order = await  _orderService.CreateOrderAsync(buyerEmail, model.BasketId, model.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiResponse(400, "There Is a Problem With Your Order!!!!!"));
            var result =  _mapper.Map<Core.Models.Order.Order,OrderToReturnDto>(order);
            return Ok(result);
        }

        #endregion

        #region GetOrdersForSpecificUser 
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet] // Get : /api/Orders
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForSpecificUserAsync(buyerEmail);
            if (orders is null) return BadRequest(new ApiResponse(400, "There Is No Order For U "));
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }


        #endregion

        #region GetOrderByIdForSpecificUser
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]  // Get : /api/Orders/21
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersByIdForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForSpecificUserAsync(buyerEmail,id);
            if (order is null) return BadRequest(new ApiResponse(400, $"There Is No Order With Id :{id} For U "));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        #endregion

        #region Get All DeliveryMethod

        [HttpGet("DeliveryMethods")]// Get : /api/Orders/DeliveryMethods
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var deliveryMethods =  await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(deliveryMethods);
        }

        #endregion





    }
}
