namespace AllSopFoodService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Services;
    using AllSopFoodService.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) => this._categoryService = categoryService;

        [HttpGet("get-all-categories")]
        public IActionResult GetAllCategories(string? sortBy, string? searchString, int? pageNum)
        {
            try
            {
                var allCategories = this._categoryService.GetAllCategories(sortBy, searchString, pageNum);

                return this.Ok(allCategories);
            }
            catch (Exception)
            {

                return this.BadRequest("Sorry, we could not load the Categories");
            }

        }

        [HttpPost("add-category")]
        public IActionResult AddCategory([FromBody] CategoryVM category)
        {
            this._categoryService.CreateCategory(category);
            return this.Ok();
        }

        [HttpGet("get-categories-with-products-incart/{id}")]
        public IActionResult GetCategoryData(int id)
        {
            var response = this._categoryService.GetCategoryData(id);

            return this.Ok(response);
        }


        [HttpDelete("delete-category/{id}")]
        public IActionResult DeleteCategoryById(int id)
        {
            this._categoryService.DeleteCategoryById(id);

            return this.Ok();
        }
    }
}
