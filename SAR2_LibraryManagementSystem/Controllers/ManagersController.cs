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
        [HttpGet]
        public IActionResult GetAllManagers()
        {
            var manager = _managerDAL.GetAllManagers();
            return Ok(manager);

        }

        [HttpPut]
        public IActionResult UpdateManager(Managers managers)
        {
            if (managers.mId <= 0)
                return BadRequest("Invalid Manager Id");

            _managerDAL.UpdateManager(managers);
            return Ok(new { succes = true, message = "Managers updated successfully." });



        }
        [HttpDelete("{mId}")]
        public IActionResult DeleteManager(int mId)
        {
            if (mId <= 0)
            { return BadRequest("Invalid Id"); }

            _managerDAL.DeleteManager(mId);
            return Ok(new { succes = true, message = "Manager Deleted Succesfully" });
        }
    }

}
