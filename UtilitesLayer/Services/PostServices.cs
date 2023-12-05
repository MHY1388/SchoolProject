using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.Mapppers;
using UtilitesLayer.Utilities;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public class PostServices : IPostServices
    {
        private readonly IGenericRepository<Post> _postRepository;
        private readonly ApplicationDbContext dbContext;
        private readonly FileManager fileManager;

        public PostServices(ApplicationDbContext dbContext, FileManager fileManager)
        {

            this._postRepository = new GenericRepository<Post>(dbContext);
            this.dbContext = dbContext;
            this.fileManager = fileManager;
        }

        public async Task<OperationResult> CreatePost(CreatePostDto post)
        {
            var path = await fileManager.SaveFile(post.Image, DirectoryPath.MediaImages, DirectoryPath.BucketName);
            return _postRepository.Create(post.MapToPost(path)).Result;
        }

        public OperationResult DeletePost(int postId)
        {
            var result = _postRepository.Get(postId).Result.ImagePath;
           fileManager.DeleteFile(result, DirectoryPath.MediaImages, DirectoryPath.BucketName).Wait();
            return _postRepository.Delete(postId).Result;
        }

        public OperationResult UpdatePost(PostDto post)
        {
            var data = post.MapToPost();

            return _postRepository.Update(data).Result;
        }

        public List<PostDto> GetPosts()
        {
            List<Expression<Func<Post, dynamic>>> includes = new (){a=>a.Category};
            return _postRepository.GetAllWithInclude(includes).Result.Select(a=>a.MapToPostDto()).ToList();
        }

        public PostDto GetPost(int postId)
        {
            //_postRepository.Get(postId).Result.MapToPostDto()
            return dbContext.Posts.AsNoTracking().Include(a=>a.Category).FirstOrDefault(a=>a.Id==postId)?.MapToPostDto();
        }

        public List<PostDto> GetPosts(Expression<Func<Post, bool>> filter)
        {
            List<Expression<Func<Post, dynamic>>> includes = new() { a => a.Category };
            
            return _postRepository.FindAllWithInclude(filter,includes).Result.Select(a=>a.MapToPostDto()).ToList();
        }

        public PostDto GetPost(Expression<Func<Post, bool>> filter)
        {
            List<Expression<Func<Post, dynamic>>> includes = new() { a => a.Category };

            return _postRepository.FindWithInclude(filter, includes).Result.MapToPostDto();
        }

        public Paggination<PostDto> GetPaggination(int page, int pageSize)
        {
            List<Expression<Func<Post, dynamic>>> includes = new() { a => a.Category };

            var data =  _postRepository.GetPagginationWithInclude(size: pageSize, page: page,includes:includes).Result;
            return new Paggination<PostDto>()
            {
                CurrentPage = data.CurrentPage, GetSize = data.GetSize, PageCount = data.PageCount,
                Objects = data.Objects.Select(a => a.MapToPostDto()).ToList()
            };
        }

        public Paggination<PostDto> GetPaggination(int page, int pageSize, string title, string slug)
        {
            List<Expression<Func<Post, dynamic>>> includes = new() { a => a.Category };

            var data = _postRepository.GetPagginationWithInclude(size: pageSize, page: page,expression:a=>a.Name.Contains(title)||a.Slug.Contains(slug),includes:includes).Result;
            return new Paggination<PostDto>()
            {
                CurrentPage = data.CurrentPage,
                GetSize = data.GetSize,
                PageCount = data.PageCount,
                Objects = data.Objects.Select(a => a.MapToPostDto()).ToList()
            };
        }

        public bool SlugExtsts(string slug)
        {
            return _postRepository.Any(a => a.Slug == slug).Result;
        }

        public Paggination<PostDto> GetPaggination(int page, int pageSize, string title, string slug, string category)
        {
            List<Expression<Func<Post, dynamic>>> includes = new() { a => a.Category };

            var data = _postRepository.GetPagginationWithInclude(size: pageSize, page: page, expression: a => a.Name.Contains(title) || a.Slug.Contains(slug)||a.Category.Slug==category, includes: includes).Result;
            return new Paggination<PostDto>()
            {
                CurrentPage = data.CurrentPage,
                GetSize = data.GetSize,
                PageCount = data.PageCount,
                Objects = data.Objects.Select(a => a.MapToPostDto()).ToList()
            };
        }

        public Paggination<PostDto> GetPaggination(int page, int pageSize, string category)
        {
            List<Expression<Func<Post, dynamic>>> includes = new() { a => a.Category };

            var data = _postRepository.GetPagginationWithInclude(size: pageSize, page: page, expression:a=>a.Category.Slug== category, includes: includes).Result;
            return new Paggination<PostDto>()
            {
                CurrentPage = data.CurrentPage,
                GetSize = data.GetSize,
                PageCount = data.PageCount,
                Objects = data.Objects.Select(a => a.MapToPostDto()).ToList()
            };
        }

        public async void VisitPost(int id)
        {
           (await dbContext.Posts.FindAsync(id)).Visit += 1;
        }
    }
}
