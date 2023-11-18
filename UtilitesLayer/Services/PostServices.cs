using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
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
        public PostServices(ApplicationDbContext dbContext)
        {
            _postRepository = new GenericRepository<Post>(dbContext);
        }

        public OperationResult CreatePost(CreatePostDto post)
        {
            var path = FileManager.SaveFile(post.Image, DirectoryPath.MediaImages);
            return _postRepository.Create(post.MapToPost(path)).Result;
        }

        public OperationResult DeletePost(int postId)
        {
            throw new NotImplementedException();
        }

        public OperationResult UpdatePost(PostDto post)
        {
            throw new NotImplementedException();
        }

        public List<PostDto> GetPosts()
        {
            throw new NotImplementedException();
        }

        public PostDto GetPost(int postId)
        {
            throw new NotImplementedException();
        }

        public List<PostDto> GetPosts(Expression<Func<Post, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public PostDto GetPost(Expression<Func<Post, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Paggination<PostDto> GetPaggination(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Paggination<PostDto> GetPaggination(int page, int pageSize, string title, string slug)
        {
            throw new NotImplementedException();
        }
    }
    public interface IPostServices
    {
        public OperationResult CreatePost(CreatePostDto post);
        public OperationResult DeletePost(int postId);
        public OperationResult UpdatePost(PostDto post);
        public List<PostDto> GetPosts();
        public PostDto GetPost(int postId);
        public List<PostDto> GetPosts(Expression<Func<Post, bool>> filter);
        public PostDto GetPost(Expression<Func<Post, bool>> filter);
        public Paggination<PostDto> GetPaggination (int page, int pageSize);
        public Paggination<PostDto> GetPaggination(int page, int pageSize, string title, string slug);

    }
   
}
