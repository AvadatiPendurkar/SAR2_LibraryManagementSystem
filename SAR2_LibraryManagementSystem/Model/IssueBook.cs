namespace SAR2_LibraryManagementSystem.Model
{
    public class IssueBook
    {
        public int issueId {  get; set; }
        public int userId { get; set; }
        public int bookId { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime dueDate { get; set; }
        public DateTime returnDate { get; set; }
        public int bookQty { get; set; }
        public string status { get; set; }
    }

    public class IssueBookReportDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalBooksIssued { get; set; }
    }

    public class GenreRecommendation
    {
        public int GenreId { get; set; }
        public string Genre { get; set; }
        public int TotalIssuedBooks { get; set; }
    }
}
