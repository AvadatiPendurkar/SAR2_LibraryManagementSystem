using System.ComponentModel.DataAnnotations;

namespace SAR2_LibraryManagementSystem.Model
{
    public class Login
    {
        [Required]
        
        public string email {  get; set; }
        [Required]

        public string password { get; set; }
    }
}
