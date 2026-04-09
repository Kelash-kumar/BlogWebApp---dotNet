using Server.DTOs.PostDtos;
using Server.Helpers;
using Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Server.Controllers
{
    public class PostsController : BaseApiController
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "desc"

            )
        {
            var result = await _postService.GetAllPostsAsync(paginationParams, search, sortBy, sortDirection);

            return ApiOk(result, "Posts Fetched Successfully.");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDto dto)
        {

            if (!ModelState.IsValid)
            {
                return ApiValidationError(ModelState);
            }

            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(authorId))
            {
                return ApiUnauthorized("User identifier not found in claims.");
            }
            dto.AuthorId = int.Parse(authorId);
            var result = await _postService.CreatePost(dto);

            return ApiOk(result, "Post Created Successfully");
        }

        [Authorize]
        [HttpPut("{uid}")]
        public async Task<IActionResult> Update([FromRoute] Guid uid, [FromBody] UpdatePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiValidationError(ModelState);
            }

            var result = await _postService.UpdatePost(uid, dto);
            return ApiOk(result, "Post Updated Successfully");
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid uid)
        {
            var category = await _postService.GetPostByIdAsync(uid);
            return ApiOk(category, "Category fetched successfully");
        }

        [Authorize]
        [HttpDelete("{uid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid uid)
        {
            await _postService.DeletePost(uid);
            return ApiNoContent("Post Deleted Successfully");
        }
    }
}
