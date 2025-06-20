namespace SAR2_LibraryManagementSystem.Model
{
    public class Users
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public string mobileNo { get; set; }
        //public bool IsAuthorized { get; set; } = false;

    }

    public class AddToWishlist
    {
        public int wishlistId { get; set; }
        public int userId { get; set; }
        public List<int> bookId { get; set; }
        public DateTime addedDate { get; set; }
    }
    public class WishlistRemove
    {
        public int WishlistId { get; set; }
        public int bookId { get; set; }
        public string bookName { get; set; }
        public string authorName { get; set; }
        public string isbn { get; set; }
        public int  genreId { get; set; }
        public int quantity { get; set; }
        public string Base64Image { get; set; }
    }



}
