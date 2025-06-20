using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly CategoryDAL _categoryDAL;

        public CategoryController(CategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;
        }

        [HttpGet("ViewAllBooks")]
        public IActionResult ViewAllBooks()
        {
            var categories = _categoryDAL.GetGenres();
            return Ok(categories);
        }

        [HttpPost("AddGenre")]
        // [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddGenre(Category category)
        {
            _categoryDAL.AddGenre(category);
            return Ok(new { message = "Category added successfully." });
        }

        [HttpDelete("delete/{Id}")]
        //[Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteGenre(int Id)
        {
            if (Id <= 0)
                return BadRequest("Invalid Category ID.");

            _categoryDAL.DeleteGenre(Id);
            return Ok(new { success = true, message = "category deleted successfully." });
        }
    }
}
