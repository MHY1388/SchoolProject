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
                ImagePath = imagepath, KeyWords = post.KeyWords, IsSpecial = post.IsSpecial, Slug = post.Slug.GenerateSlug()
            };
        }   
    }
}
