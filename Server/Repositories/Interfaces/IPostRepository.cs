using Server.Helpers;
using Server.Models;

namespace Server.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> CreatePost(Post post);
        Task<(List<Post>, int totalRecords)> GetAllPostsAsync(
            PaginationParams paginationParams,
            string? search = null,
            string? sortBy = "createdAt",
            string? sortDirection = "desc"
            );
        Task<List<string>> GetAllPostSlugsAsync();
        Task<Post?> GetPostByIdAsync(Guid uid);
        Task<Post> UpdatePost(Post post);
        Task<bool> DeletePost(Guid uid);
    }
}
