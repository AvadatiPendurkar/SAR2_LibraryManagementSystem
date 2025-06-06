using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Repositories.Interfaces
{
    public interface IIssueBookRepository
    {
        void AddIssueBooks(IssueBook issueBook);
        void UpdateIssueBooks(IssueBook issueBook);
        List<IssueBook> ViewIssuesBooks();
        IssueBook ViewIssueBooksById(int id);
        void ReturnIssuedBook(int issueId);
    }
}
