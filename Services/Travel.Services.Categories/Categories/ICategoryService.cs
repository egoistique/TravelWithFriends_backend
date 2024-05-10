namespace Travel.Services.Categories;

public interface ICategoryService
{
    Task<IEnumerable<CategoryModel>> GetAll();
    Task<CategoryModel> GetById(Guid id);
    Task Delete(Guid id);
}
