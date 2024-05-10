using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Common.Security;
using Travel.Services.Activities;
using Travel.Services.Categories;
using Travel.Services.Logger;

namespace Travel.Api.App;

[ApiController]
//[Authorize]
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
    //[Authorize(AppScopes.TripsRead)]
    public async Task<IEnumerable<CategoryModel>> GetAll()
    {
        var result = await categoryService.GetAll();

        return result;
    }

    [HttpGet("{id:Guid}")]
    //[Authorize(AppScopes.TripsRead)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await categoryService.GetById(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id:Guid}")]
    //[Authorize(AppScopes.TripsWrite)]
    public async Task Delete([FromRoute] Guid id)
    {
        await categoryService.Delete(id);
    }
}

