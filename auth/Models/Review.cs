namespace api.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int BookID { get; set; }
        public float Rating { get; set; }
        public string Description { get; set;}
        public string UserID { get; set; }
    }
}
