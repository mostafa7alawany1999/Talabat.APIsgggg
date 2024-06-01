using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services.Interfaces;

namespace Talabat.APIs.Controllers
{

    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController
            (
            UserManager<AppUser> userManager 
            , SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
           _userManager = userManager;
           _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }


        #region Register

        [HttpPost("Register")] // Post : /api/accounts/Register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "email is alread exists"));
            }



            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split("@")[0],
            };
           var Result =   await _userManager.CreateAsync(user, model.PassWord);

            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));

            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName, 
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user,_userManager)
            };
            
            return Ok(ReturnedUser);
        }



        #endregion


        #region Login 
        [AllowAnonymous]
        [HttpPost("Login")] // Post : /api/accounts/Login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {

           var user =  await _userManager.FindByEmailAsync(model.Email);

            if (user is null) return Unauthorized(new ApiResponse(401));

           var result =  await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if(!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });

        }



        #endregion


        #region Get Current User

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail =   User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }

        #endregion

        #region Get Current User By Address

        [Authorize]
        [HttpGet("CurrentUserAddress")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            // var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //  var user = await _userManager.FindByEmailAsync(userEmail);
            var user = await _userManager.FindUserWithAddressAsync(User);
          var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(MappedAddress);
        }

        #endregion


        #region Update User Address

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto model)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var address = _mapper.Map<AddressDto, Address>(model);
            user.Address = address;

            var Result =  await _userManager.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(model);
        }



        #endregion


        #region Validate Dublicate Email

        [HttpGet("emailExists")] // Get :: 
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            //var user = await _userManager.FindByEmailAsync(email);

            //if (user is null) return false;

            //return true;


            return await _userManager.FindByEmailAsync(email) is not null;

        }


        #endregion



    }
}
