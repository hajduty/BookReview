using api.Identity;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IConfiguration _config;

        public BookController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        [HttpPost]
        [Route("addbook")]
        public ActionResult<Book> AddBook(Book book)
        {
            var jwtToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            var DataAccess = new DbHelper();

            DataAccess.InsertData(book, "Books");

            // Return a 200 OK response with a message
            return Ok(book);
        }

        [HttpGet]
        [Route("books")]
        public List<Book> GetBooks()
        {
            var dataAccess = new DbHelper();
            return dataAccess.GetData<Book>();
        }

        [HttpGet]
        [Route("bookinfo")]  // Include the bookId in the URL
        public ActionResult<string> BookInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // Return a 400 Bad Request response if 'id' is not provided
                return BadRequest("Id parameter is required.");
            }

            var dataAccess = new DbHelper();

            Book book = dataAccess.GetData<Book>("Books", "BookID = @BookID", new { BookID = id });

            // Return a 200 OK response with a message
            return Ok(book);
        }
    }
}
