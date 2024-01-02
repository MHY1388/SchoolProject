using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.Utilities;

namespace UtilitesLayer.Mapppers
{
    public static class CategoryMapper
    {
        public static Category MapToCategory(this CreateCategoryDto model)
        {
            return new Category()
            {
                Name = model.Name, Slug = model.Slug.GenerateSlug()
            };
        }

        public static Category MapToCategory(this CategoryDto model)
        {
            var data =  new Category()
            {
                Id = model.Id, IsDeleted = model.IsDeleted, Name = model.Name,
                Slug = model.Slug, Updated = DateTime.Now
            };
            if (model.Posts is not null)
            {
                data.Posts = model.Posts;
            }
            return data;
        }

        public static CategoryDto MapToCategoryDto(this Category model)
        {
            var data = new CategoryDto()
            {
                Created = model.Created,
                Id = model.Id,
                IsDeleted = model.IsDeleted,
                Name = model.Name,
                Slug = model.Slug,
            };
            if (model.Posts is not null)
            {
                data.Posts = model.Posts;
            }
            return data;
        }
    }
}
