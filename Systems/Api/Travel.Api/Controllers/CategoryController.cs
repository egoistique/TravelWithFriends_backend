using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Travel.Common.Security;
using Travel.Services.Activities;
using Travel.Services.Categories;
using Travel.Services.Logger;

namespace Travel.Api.App;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Product")]
[Route("v{version:apiVersion}/[controller]")]
public class CategoryController : Controller
{
    private readonly IAppLogger logger;
    private readonly ICategoryService categoryService;

    public CategoryController(IAppLogger logger, ICategoryService categoryService)
    {
        this.logger = logger;
        this.categoryService = categoryService;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Get all categories", Description = "")]
    public async Task<IEnumerable<CategoryModel>> GetAll()
    {
        var result = await categoryService.GetAll();

        return result;
    }

    [HttpGet("{id:Guid}")]
    [SwaggerOperation(Summary = "Get category by id", Description = "")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await categoryService.GetById(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id:Guid}")]
    [SwaggerOperation(Summary = "Delete category", Description = "")]
    public async Task Delete([FromRoute] Guid id)
    {
        await categoryService.Delete(id);
    }
}

