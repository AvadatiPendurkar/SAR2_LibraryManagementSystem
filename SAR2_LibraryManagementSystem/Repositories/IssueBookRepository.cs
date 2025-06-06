using SAR2_LibraryManagementSystem.Model;
using SAR2_LibraryManagementSystem.Repositories.Interfaces;

namespace SAR2_LibraryManagementSystem.Repositories
{
    public class IssueBookRepository : IIssueBookRepository
    {
        private readonly IssueBookDAL _issueBookDAL;

        public IssueBookRepository(IssueBookDAL issueBookDAL)
        {
            _issueBookDAL = issueBookDAL;
        }

        public void AddIssueBooks(IssueBook issueBook) => _issueBookDAL.AddIssueBooks(issueBook);

        public void UpdateIssueBooks(IssueBook issueBook) => _issueBookDAL.UpdateIssueBooks(issueBook);

        public List<IssueBook> ViewIssuesBooks() => _issueBookDAL.ViewIssuesBooks();

        public IssueBook ViewIssueBooksById(int id) => _issueBookDAL.ViewIssueBooksById(id);

        public void ReturnIssuedBook(int issueId) => _issueBookDAL.ReturnIssuedBook(issueId);

        // public void DeleteIssueBook(int issueId) => _issueBookDAL.DeleteIssueBook(issueId); // optional

    }
}
