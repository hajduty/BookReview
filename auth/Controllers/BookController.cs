using api.Models;
using api.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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

        [HttpPost]
        [Route("addbook")]
        [Authorize(Policy = "LocalhostOnly")]
        public ActionResult<Book> AddBook(Book book)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("ReviewAuth").ToString());



            return Ok(book);
        }

        [HttpGet]
        [Route("bookinfo")]  // Include the bookId in the URL
        public ActionResult<string> GetMessage(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // Return a 400 Bad Request response if 'id' is not provided
                return BadRequest("Id parameter is required.");
            }

            // Perform your logic here
            Book book = GetBookFromId(id);
            // Return a 200 OK response with a message
            return Ok($"");
        }

        public Book GetBookFromId(string bookId)
        {
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("ReviewAuth").ToString()))
            {
                connection.Open();

                string sqlQuery = "SELECT BookID, Author, Title, Description, Rating, PublicationYear " +
                                  "FROM Books " +
                                  "WHERE BookID = @BookID;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@BookID", bookId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Book
                            {
                                BookID = reader["BookID"].ToString(),
                                Author = reader["Author"].ToString(),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                Rating = Convert.ToSingle(reader["Rating"]),
                                PublicationYear = reader["PublicationYear"].ToString()
                            };
                        }
                    }
                }
            }

            // Return null if the book with the given ID is not found
            return null;
        }
    }
}
