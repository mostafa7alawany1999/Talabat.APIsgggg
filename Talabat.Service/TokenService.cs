﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services.Interfaces;

namespace Talabat.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> _userManager)
        {

            //1.Header

            #region Payload Private Claims(Name, id , email,........)
            
            var AuthClaims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var UserRoles = await  _userManager.GetRolesAsync(user);
            foreach (var Role in UserRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
            }
            #endregion

            #region Signature (1.Key , )
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            #endregion

            #region Ctreate Token


            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: AuthClaims,
                signingCredentials: new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature)
                );
               

            #endregion

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
