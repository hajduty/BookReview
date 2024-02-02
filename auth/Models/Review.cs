namespace api.Models
{
    public class Review
    {
        public string ReviewID { get; set; }
        public string BookID { get; set; }
        public float Rating { get; set; }
        public string Description { get; set;}
        public string UserID { get; set; }

    }
}
