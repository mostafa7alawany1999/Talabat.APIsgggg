using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Models;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;
using Talabat.APIs.Extensions;
using StackExchange.Redis;
using Talabat.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services.Interfaces;
using Talabat.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          

            #region Configure Services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });



            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequireDigit = true;
                //options.Password.RequiredUniqueChars = 2;
                //options.Password.RequireNonAlphanumeric = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            builder.Services.AddScoped<ITokenService, TokenService>();
            // Open Connection with Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddAuthentication(options =>
                            {
                                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                            })
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters()
                                {
                                    ValidateIssuer = true,
                                    ValidIssuer =builder.Configuration["JWT:ValidIssuer"],
                                    ValidateAudience = true,
                                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                                };
                            });

            // Method Clean Code (Write All Configure in Class ApplicationServiceExtensions)
            builder.Services.AddApplicationServiceExtensions();





            #endregion

            var app = builder.Build();

            #region Update Database  - DataSeed

            var scope =  app.Services.CreateScope();

            var service =  scope.ServiceProvider;

            var _context =  service.GetRequiredService<StoreDbContext>();

            // Ask CLR For Creating Object From StoreDbContext Explicitly 

            var _IdentityDbContext = service.GetRequiredService<AppIdentityDbContext>();


            var loggerFactory =  service.GetRequiredService<ILoggerFactory>();

            try
            {

                #region  Update Database (StoreDbContext) And DataSeeding

                await _context.Database.MigrateAsync();

                // Data Seeding
                await StoreDbContextSeed.SeedAsync(_context);
                #endregion

                #region Update Database (AppIdentityDbContext) And DataSeeding
                await _IdentityDbContext.Database.MigrateAsync();

                var _UserManager =  service.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(_UserManager);
                #endregion
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                 logger.LogError(ex,"An Error Has Been Occured During Appling The Migrations");
            }

            #endregion

            #region Configure 


            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionMiddlewares>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            #endregion


            app.Run();
        }
    }
}
