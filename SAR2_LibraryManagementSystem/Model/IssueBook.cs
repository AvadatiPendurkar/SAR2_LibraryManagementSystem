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
}
