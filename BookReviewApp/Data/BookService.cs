using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BookReviewApp.Data
{
    // BookService.cs
    public class BookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            // Replace the endpoint with the actual URL of your books API
            return await _httpClient.GetFromJsonAsync<List<Book>>("https://localhost:7115/api/Book/books");
        }
    }
}