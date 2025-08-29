namespace SAR2_LibraryManagementSystem.Model
{
    public class Books
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public string authorName { get; set; }
        public string isbn { get; set; }
        public int genreId { get; set; }
        public int quantity { get; set; }
        public string Base64Image { get; set; }

        //joined field from Category table
        public string genre { get; set; }

        public double average_rating { get; set; }
        public int total_ratings { get; set; }
        public int ratings { get; set; }
        public bool isInWishlist { get; set; }

        public DateTime LaunchDate { get; set; }
    }

    public class UpdateBookDto
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public string authorName { get; set; }
        public string isbn { get; set; }
        public int genreId { get; set; }
        public int quantity { get; set; }
        public string Base64Image { get; set; }
    }

    public class BookRating
    {
        public int userId { get; set; }
        public int bookId { get; set; }
        public int ratings { get; set; } // 1 to 5 stars
    }

}
