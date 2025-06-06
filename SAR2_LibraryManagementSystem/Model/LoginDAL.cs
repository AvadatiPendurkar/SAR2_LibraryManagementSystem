using Microsoft.AspNetCore.Http.HttpResults;

namespace SAR2_LibraryManagementSystem.Model
{

   public interface IRepo<T>
    {
        void Save(T data);
    }
    public class Repository<T> : IRepo<T>
    {
        public void Save(T data)
        {
            string insert = "";
            if (data is Managers)
            {
                insert = "";
            }
            //throw new NotImplementedException();
        }
    }

    public class LoginDAL
    {
        private readonly string _connectionString;

        //Connection String
        public LoginDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //public bool Login(string username, string password)
        //{
        //    return;
        //}
    }
}
