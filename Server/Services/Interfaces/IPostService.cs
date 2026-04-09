using Server.DTOs.PostDtos;
using Server.Helpers;

namespace Server.Services.Interfaces
{
    public interface IPostService
    {
        Task<PostResponseDto?> CreatePost(CreatePostDto postDto);
        Task<PostResponseDto?> GetPostByIdAsync(Guid uid);
        Task<PagedResult<PostResponseDto>> GetAllPostsAsync(
            PaginationParams pagination,
            string? search = null,
            string? sortBy = "createdAt",
            string? sortOrder = "asc"
            );
        Task<PostResponseDto?> UpdatePost(Guid uid, UpdatePostDto postDto);
        Task<bool> DeletePost(Guid uid);
    }
}
