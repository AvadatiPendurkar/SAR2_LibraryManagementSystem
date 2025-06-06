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
        public bool IsAuthorized { get; set; } = false;

    }

    //public class Demo
    //{
    //    public int Id { get; set; }
    //    public string fname { get; set; }
    //    public string status { get; set; }
    //}
}
