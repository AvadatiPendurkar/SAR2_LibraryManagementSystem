  using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataAccessLayer _dataAccessLayer;

        public UsersController(DataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        [HttpPost]
        public IActionResult AddUser(Users user)
        {
            _dataAccessLayer.AddUser(user);
            return Ok("User added successfully");
        }
    }
}
