using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.Core.Models;
using Talabat.Core.Repositories.Interfaces;

namespace Talabat.APIs.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }





        // Get By ID 
        #region Get By ID 

        [HttpGet] // Get : /api/basket/id==11
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket is null) return new CustomerBasket() { Id = id };
            return Ok(basket);
                
        
        }


        #endregion

        // Delete

        #region Delete

        [HttpDelete] // Delete : /api/basket
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);

        }


        #endregion

        // Create && Upate

        #region Create && Upate

        [HttpPost] // Post : /api/basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedbasket = _mapper.Map<CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedbasket);
            if(CreatedOrUpdatedBasket is null)
            {
                return BadRequest(new ApiResponse(400));
            }

            return Ok(CreatedOrUpdatedBasket);

        }


        #endregion

    }
}
