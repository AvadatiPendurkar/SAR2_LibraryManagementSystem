using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly ManagerDAL _managerDAL;

        public ManagersController(ManagerDAL managerDAL)
        {
            _managerDAL = managerDAL;
        }

        [HttpPost]
        public IActionResult AddManagers(Managers manager)
        {

            _managerDAL.AddManagers(manager);
            return Ok("Manager added successfully");

        }
    }
}
