using System.Linq.Expressions;
using DataLayer.Entities;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.Utilities;

namespace UtilitesLayer.Services;

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
    public bool SlugExtsts(string slug);
}