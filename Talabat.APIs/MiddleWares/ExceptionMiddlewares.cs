using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddleWares
{
    public class ExceptionMiddlewares
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddlewares> _logger;
        private readonly IHostEnvironment _evn;

        public ExceptionMiddlewares(RequestDelegate next , ILogger<ExceptionMiddlewares> logger , IHostEnvironment evn)
        {
            _next = next;
            _logger = logger;
            _evn = evn;
        }


        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                // Log Exception Database [Production]

                context.Response.ContentType = "application/json";
                context.Response.StatusCode =(int)HttpStatusCode.InternalServerError;

                var response = _evn.IsDevelopment() ?
                               new ApiServerErrorResponse(500, ex.Message, ex.StackTrace.ToString())
                               : new ApiServerErrorResponse(500);

                var json = JsonSerializer.Serialize(response);

               await context.Response.WriteAsync(json);
            }





        }
    }
}
