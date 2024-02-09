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
        public async Task<Book> GetRandomBook()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Book>("https://localhost:7115/api/Book/randombook");
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error fetching random book: {ex.Message}");
                throw; // Rethrow the exception
            }
        }
        public async Task<Book> GetBookFromId(int id)
        {
            // Replace the endpoint with the actual URL of your books API
            return await _httpClient.GetFromJsonAsync<Book>("https://localhost:7115/api/Book/bookinfo?id=" + id);
        }
    }
}