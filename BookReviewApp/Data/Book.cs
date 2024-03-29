﻿namespace BookReviewApp.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        public int PublicationYear { get; set; }
    }
}