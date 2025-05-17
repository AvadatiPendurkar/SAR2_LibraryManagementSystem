namespace SAR2_LibraryManagementSystem.Model
{
    public class LoginDAL
    {
        private readonly string _connectionString;

        //Connection String
        public LoginDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool Login(string username, string password)
        {

        }
    }
}
