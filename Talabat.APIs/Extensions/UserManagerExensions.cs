using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Models.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserManagerExensions
    {

        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
           var userEmail =  user.FindFirstValue(ClaimTypes.Email);
           var Result = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(u => u.Email == userEmail);
            return Result;
        }




    }
}
