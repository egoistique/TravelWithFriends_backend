using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Travel.Common.Security;
using Travel.Services.Activities;
using Travel.Services.Categories;
using Travel.Services.Logger;

namespace Travel.Api.App;

/// <summary>
/// Controller for managing categories.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Product")]
[Route("v{version:apiVersion}/[controller]")]
public class CategoryController : Controller
{
    private readonly IAppLogger logger;
    private readonly ICategoryService categoryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="categoryService">The category service instance.</param>
    public CategoryController(IAppLogger logger, ICategoryService categoryService)
    {
        this.logger = logger;
        this.categoryService = categoryService;
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>A list of all categories.</returns>
    [HttpGet("")]
    [SwaggerOperation(Summary = "Get all categories", Description = "Retrieves all categories.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<CategoryModel>))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(401, "Unauthorized", typeof(string))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IEnumerable<CategoryModel>> GetAll()
    {
        var result = await categoryService.GetAll();

        return result;
    }

    /// <summary>
    /// Gets a category by ID.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>The category with the specified ID.</returns>
    [HttpGet("{id:Guid}")]
    [SwaggerOperation(Summary = "Get category by id", Description = "Retrieves a category by its ID.")]
    [SwaggerResponse(200, "Success", typeof(CategoryModel))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(401, "Unauthorized", typeof(string))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await categoryService.GetById(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Deletes a category.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id:Guid}")]
    [SwaggerOperation(Summary = "Delete category", Description = "Deletes a category by its ID.")]
    [SwaggerResponse(204, "No Content")]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(401, "Unauthorized", typeof(string))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task Delete([FromRoute] Guid id)
    {
        await categoryService.Delete(id);
    }
}

