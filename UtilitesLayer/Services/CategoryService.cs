using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.Mapppers;
using UtilitesLayer.Utilities;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
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
    public class CategoryServices:ICategoryServices
    {
        private readonly IGenericRepository<Category> _repository;
        public CategoryServices(ApplicationDbContext context)
        {
            _repository = new GenericRepository<Category>(context);
        }
        public OperationResult CreateCategory(CreateCategoryDto category)
        {
            return _repository.Create(category.MapToCategory()).Result;
        }

        public OperationResult DeleteCategory(int id)
        {
            var result = _repository.Delete(id).Result;
            return result;
        }

        public OperationResult UpdateCategory(CategoryDto category)
        {
            return _repository.Update(category.MapToCategory()).Result;
        }

        public CategoryDto GetCategory(int id)
        {
            return  _repository.Get(id).Result.MapToCategoryDto();
        }

        public CategoryDto GetCategory(Expression<Func<Category, bool>> expression)
        {
            return _repository.Find(expression).Result.MapToCategoryDto();
        }

        public List<CategoryDto> GetCategories()
        {
            return _repository.GetAll().Result.Select(a => a.MapToCategoryDto()).ToList();
        }

        public List<CategoryDto> GetCategories(Expression<Func<Category, bool>> expression)
        {
            return _repository.FindAll(expression).Result.Select(a => a.MapToCategoryDto()).ToList();
        }

        public Paggination<CategoryDto> GetPaggination(int page, int pageSize, string name=null, string slug=null)
        {
            Paggination<Category> paggination;
            if ((!name.IsNullOrEmpty()) || (!slug.IsNullOrEmpty()))
            {
                 paggination = _repository
                    .GetPaggination(pageSize, a => a.Name.Contains(name) || a.Slug.Contains(slug), page).Result;
            }
            else
            {
                paggination = _repository.GetPaggination(pageSize, page).Result;
            }
            return new Paggination<CategoryDto>(){CurrentPage = paggination.CurrentPage, GetSize = paggination.GetSize, PageCount = paggination.PageCount, Objects = paggination.Objects.Select(a=>a.MapToCategoryDto()).ToList()};
        }

        public bool SlugExists(string slug)
        {
            return _repository.Any(a => a.Slug == slug).Result;
        }

        public bool NameExists(string name)
        {
            return _repository.Any(a => a.Name == name).Result;
        }
    }
}
