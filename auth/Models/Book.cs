namespace api.Models
{
    public class Book
    {
        public string BookID { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public string PublicationYear { get; set; }
    }
}