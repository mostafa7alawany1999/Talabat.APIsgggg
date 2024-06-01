using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
  
    public class BuggyController : BaseApiController
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context)
        {
            _context = context;
        }



        #region before Handling

        #region NotFound
        //[HttpGet("NotFound")] // Get :: /api/Buggy/NotFound
        // public ActionResult GetNotFoundRequest ()
        // {
        //    var product =  _context.Products.Find(1000);

        //    if (product is null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(product);
        // }
        #endregion

        #region ServerError
        //[HttpGet("ServerError")] // Get :: /api/Buggy/ServerError
        //public ActionResult GetServerError()
        //{
        //    var product = _context.Products.Find(1000);

        //    var result =  product.ToString(); // Will Throw Exception [Null ReferenceException]

        //    return Ok(result);
        //}
        #endregion

        #region BadRequest
        //[HttpGet("BadRequest")] // Get :: /api/Buggy/BadRequest
        //public ActionResult GetBadRequest()
        //{
        //    return BadRequest();
        //}
        #endregion

        #region BadRequest/{id}
        //[HttpGet("BadRequest/{id}")] // Get :: /api/Buggy/BadRequest/5
        //public ActionResult GetBadRequest(int? id) // Validtion Error
        //{
        //    return Ok();
        //}
        #endregion

        #region UnauthorizedError
        //[HttpGet("UnauthorizedError")] // Get :: /api/Buggy/UnauthorizedError
        //public ActionResult GetUnauthorizedError()
        //{
        //    return Unauthorized();
        //}
        #endregion


        #endregion


        #region  Handling EndPoint

        #region NotFound
        [HttpGet("NotFound")] // Get :: /api/Buggy/NotFound
        public ActionResult GetNotFoundRequest()
        {
            var product = _context.Products.Find(1000);

            if (product is null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(product);
        }
        #endregion

        #region ServerError
        [HttpGet("ServerError")] // Get :: /api/Buggy/ServerError
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(1000);

            var result = product.ToString(); // Will Throw Exception [Null ReferenceException]

            return Ok(result);
        }
        #endregion

        #region BadRequest
        [HttpGet("BadRequest")] // Get :: /api/Buggy/BadRequest
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        #endregion

        #region BadRequest/{id}
        [HttpGet("BadRequest/{id}")] // Get :: /api/Buggy/BadRequest/5
        public ActionResult GetBadRequest(int? id) // Validtion Error
        {
            return Ok(); // Handling in Class Program
        }

         
        #endregion

        #region UnauthorizedError
        [HttpGet("UnauthorizedError")] // Get :: /api/Buggy/UnauthorizedError
        public ActionResult GetUnauthorizedError()
        {
            return Unauthorized(new ApiResponse(401));
        }
        #endregion


        #endregion
    }
}
