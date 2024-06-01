using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Interfaces;
using Talabat.Repository.Repositories;
using Talabat.Service;


namespace Talabat.APIs.Extensions
{
    public static class ApplicationServiceExtensions
    {

        public static IServiceCollection AddApplicationServiceExtensions(this IServiceCollection services)
        {

            #region
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IPaymentService, PaymentService>();

            // Handling Validation Error
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                             .SelectMany(P => P.Value.Errors)
                                             .Select(E => E.ErrorMessage).ToArray();

                    var validtionErrorResponse = new ApivalidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validtionErrorResponse);
                };
            });



            //services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            //services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
            //services.AddScoped<IGenericRepository<ProductCategory>, GenericRepository<ProductCategory>>();

            #endregion



            return services;
        }

    }
}
