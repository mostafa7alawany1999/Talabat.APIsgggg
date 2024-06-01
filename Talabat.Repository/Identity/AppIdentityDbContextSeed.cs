using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {


        public static async Task SeedUserAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count() == 0)
            {

                var user = new AppUser()
                {
                    DisplayName = "Ramy Elsayed",
                    Email = "Ramye301@gmail.com",
                    UserName="Ramy.Elsayed",
                    PhoneNumber = "01123456543"
                };
                await _userManager.CreateAsync(user, "Pa$$W0rd");
            }
        }

    }
}
