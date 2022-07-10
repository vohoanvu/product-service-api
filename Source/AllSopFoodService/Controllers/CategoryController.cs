namespace AllSopFoodService.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService) => this.categoryService = categoryService;

        [HttpGet("get-all-categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            try
            {
                var allCategories = await this.categoryService.GetAllCategoriesAsync().ConfigureAwait(true);

                return this.Ok(allCategories);
            }
            catch (Exception)
            {

                return this.BadRequest("Sorry, we could not load the Categories");
            }

        }

        [HttpPost("add-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCategory([FromBody] CategoryVM category)
        {
            if (category == null)
            {
                return this.BadRequest(category);
            }
            this.categoryService.CreateCategory(category);
            return this.Ok();
        }

        [HttpGet("get-category-data/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategoryData(int id)
        {
            var response = this.categoryService.GetCategoryData(id);

            return this.Ok(response);
        }


        [HttpDelete("delete-category/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteCategoryById(int id)
        {
            this.categoryService.DeleteCategoryById(id);

            return this.Ok();
        }
    }
}
