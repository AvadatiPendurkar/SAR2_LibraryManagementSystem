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

        private readonly IRepo<Managers> managerRepo;

        public ManagersController(ManagerDAL managerDAL, IRepo<Managers> _managerRepo)
        {
            _managerDAL = managerDAL;
            managerRepo = _managerRepo;
        }

        [HttpPost("register")]
        public IActionResult AddManagers(Managers manager)
        {

            //_managerDAL.AddManagers(manager);
            //return Ok(new { message = "Manager registered successfully" });
            try
            {
                // your ADO.NET insert logic here...
                managerRepo.Save(manager);
                _managerDAL.AddManagers(manager);
                return Ok(new { message = "Manager registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpGet("get")]
        public IActionResult GetAllManagers()
        {
            var manager = _managerDAL.GetAllManagers();
            return Ok(manager);

        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateManager(int id, [FromBody] Managers manager)
        {
            Console.WriteLine($"Route ID: {id}, Body ID: {manager?.mId}");

            if (manager == null)
                return BadRequest(new { error = "Manager data is missing" });

            _managerDAL.UpdateManager(manager);
            return Ok(new { success = "Manager updated (even if ID mismatch)" });
        }



        [HttpDelete("delete/{mId}")]
        public IActionResult DeleteManager(int mId)
        {
            if (mId <= 0)
            { return BadRequest("Invalid Id"); }

            _managerDAL.DeleteManager(mId);
            return Ok(new { succes = true, message = "Manager Deleted Succesfully" });
        }

        [HttpGet("getById/{id}")]
        public IActionResult GetManagerById(int id)
        {
            var manager = _managerDAL.GetManagerById(id);
            if(manager == null)
            {
                return NotFound(new {message =$"Manager with ID {id} not found." });

            }
            return Ok(manager);
        }
    }
}