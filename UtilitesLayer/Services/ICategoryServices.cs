using System.Linq.Expressions;
using DataLayer.Entities;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.Utilities;

namespace UtilitesLayer.Services;

public interface ICategoryServices
{
    public OperationResult CreateCategory(CreateCategoryDto category);
    public OperationResult DeleteCategory(int id);
    public OperationResult UpdateCategory(CategoryDto category);
    public CategoryDto GetCategory(int id);
    public CategoryDto GetCategory(Expression<Func<Category, bool>> expression);
    public List<CategoryDto> GetCategories();
    public List<CategoryDto> GetCategories(Expression<Func<Category, bool>> expression);
    public Paggination<CategoryDto> GetPaggination(int page, int pageSize,string name=null, string slug=null);
    public bool SlugExists(string slug);
    public bool NameExists(string name);

}