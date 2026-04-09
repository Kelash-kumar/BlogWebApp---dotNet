using Client.Models;
using Client.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IBlogService
    {
        Task<ApiResponse<PagedResult<PostResponseDto>>> GetPostsAsync(int pageNumber = 1, int pageSize = 10, string? search = null, string? sortBy = null, string sortDirection = "desc", int? authorId = null);
        Task<ApiResponse<PostResponseDto>> GetPostByUidAsync(Guid uid);
        Task<ApiResponse<PostResponseDto>> CreatePostAsync(CreatePostDto postDto);
        Task<ApiResponse<PostResponseDto>> UpdatePostAsync(Guid uid, CreatePostDto postDto);
        Task<ApiResponse<object>> DeletePostAsync(Guid uid);
        Task<ApiResponse<PagedResult<CategoryDto>>> GetCategoriesAsync();
        Task<ApiResponse<List<CommentResponseDto>>> GetCommentsByPostIdAsync(int postId);
        Task<ApiResponse<CommentResponseDto>> CreateCommentAsync(CreateCommentDto commentDto);
    }
}
