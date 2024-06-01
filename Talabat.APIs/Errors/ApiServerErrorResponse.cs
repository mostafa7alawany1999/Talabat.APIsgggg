namespace Talabat.APIs.Errors
{
    public class ApiServerErrorResponse : ApiResponse
    {
        public string? Details { get; set; }

        public ApiServerErrorResponse(int statuscode, string? message = null, string? details =null) 
            : base(statuscode, message)
        {
            Details = details;
        }




    }
}
