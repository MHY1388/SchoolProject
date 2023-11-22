using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.Mapppers;
using UtilitesLayer.Utilities;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public class CategoryServices:ICategoryServices
    {
        private readonly IGenericRepository<Category> _repository;
        private readonly ApplicationDbContext context;

        public CategoryServices(ApplicationDbContext context)
        {
            _repository = new GenericRepository<Category>(context);
            this.context = context;
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
            //return  _repository.Get(id).Result.MapToCategoryDto();
            return context.Categories.AsNoTracking().FirstOrDefault(a => a.Id == id).MapToCategoryDto();
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
