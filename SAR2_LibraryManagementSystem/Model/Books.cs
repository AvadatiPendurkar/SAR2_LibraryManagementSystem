namespace SAR2_LibraryManagementSystem.Model
{
    public class Books
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public string authorName { get; set; }
        public string isbn { get; set; }
        public string genre { get; set; }
        public int quantity { get; set; }
        public string Base64Image { get; set; }
    }
}
