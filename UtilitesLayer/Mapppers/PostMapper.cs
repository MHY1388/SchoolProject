using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using UtilitesLayer.DTOs;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.Utilities;

namespace UtilitesLayer.Mapppers
{
    public static class PostMapper
    {
        public static Post MapToPost(this CreatePostDto post,string imagepath)
        {
            return new Post()
            {
                Name = post.Name, Description = post.Description, CategoryID = post.CategoryID, Content = post.Content,
                ImagePath = imagepath, KeyWords = post.KeyWords.Replace("،", ","), IsSpecial = post.IsSpecial, Slug = post.Slug.GenerateSlug()
            };
        }

        public static Post MapToPost(this PostDto post)
        {
            return new Post()
            {
                Id = post.Id, CategoryID = post.CategoryID, Content = post.Content, IsDeleted = post.IsDeleted,
                IsSpecial = post.IsSpecial, ImagePath = post.ImagePath, KeyWords = post.KeyWords.Replace("،", ","),
                Description = post.Description, Slug = post.Slug.GenerateSlug(), Name = post.Name
            };
        }

        public static PostDto MapToPostDto(this Post post)
        {
            var data =  new PostDto()
            {
                Id = post.Id, CategoryID = post.CategoryID, Description = post.Description, Content = post.Content,
                Created = post.Created, KeyWords = post.KeyWords, ImagePath = post.ImagePath,
                IsDeleted = post.IsDeleted, Name = post.Name, Slug = post.Slug, IsSpecial = post.IsSpecial,
                Visit = post.Visit
            };
            if (post.Category is not null)
            {
                data.Category= post.Category;
            }
            return data;
        }
    }
}
