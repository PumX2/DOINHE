using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DOINHE_BusinessObject;
using DOINHE_Repository;

namespace DOINHE1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ODataController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_categoryRepository.GetAllCategories());
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            _categoryRepository.SaveCategory(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            var existingCategory = _categoryRepository.GetCategoryById(id);
            if (existingCategory == null)
                return NotFound();

            _categoryRepository.UpdateCategory(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
                return NotFound();

            _categoryRepository.DeleteCategory(category);
            return NoContent();
        }
    }
}
