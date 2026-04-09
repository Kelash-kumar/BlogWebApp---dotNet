using Client.Models.DTOs;
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.Pages.Blog
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogService _blogService;

        public DetailsModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public PostResponseDto Post { get; set; } = new();
        public List<CommentResponseDto> Comments { get; set; } = new();

        [BindProperty]
        public CreateCommentDto NewComment { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid uid)
        {
            var postResponse = await _blogService.GetPostByUidAsync(uid);
            if (!postResponse.Success || postResponse.Data == null)
            {
                return NotFound();
            }

            Post = postResponse.Data;
            var commentsResponse = await _blogService.GetCommentsByPostIdAsync(Post.Id ?? 0);
            if (commentsResponse.Success && commentsResponse.Data != null)
            {
                Comments = commentsResponse.Data;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid uid)
        {
            if (!ModelState.IsValid)
            {
                // Re-fetch data to show the page again with errors
                var postResponse = await _blogService.GetPostByUidAsync(uid);
                if (postResponse.Success && postResponse.Data != null)
                {
                    Post = postResponse.Data;
                    var commentsResponse = await _blogService.GetCommentsByPostIdAsync(Post.Id ?? 0);
                    if (commentsResponse.Success && commentsResponse.Data != null)
                    {
                        Comments = commentsResponse.Data;
                    }
                }
                return Page();
            }

            // Set UserId if authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out int userId))
                {
                    NewComment.UserId = userId;
                }
            }

            var result = await _blogService.CreateCommentAsync(NewComment);
            if (result.Success)
            {
                return RedirectToPage(new { uid = uid });
            }

            ModelState.AddModelError(string.Empty, result.Message);
            
            // Re-fetch data on failure
            var pResp = await _blogService.GetPostByUidAsync(uid);
            if (pResp.Success && pResp.Data != null)
            {
                Post = pResp.Data;
                var cResp = await _blogService.GetCommentsByPostIdAsync(Post.Id ?? 0);
                if (cResp.Success && cResp.Data != null)
                {
                    Comments = cResp.Data;
                }
            }

            return Page();
        }
        public async Task<JsonResult> OnPostCommentAjaxAsync()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Please write a comment before submitting." });
            }

            // Set UserId if authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out int userId))
                {
                    NewComment.UserId = userId;
                }
            }

            var result = await _blogService.CreateCommentAsync(NewComment);
            
            if (result.Success)
            {
                return new JsonResult(new { success = true, message = "Comment posted successfully! It will appear after refreshing or navigating." });
            }

            return new JsonResult(new { success = false, message = result.Message });
        }
        public async Task<JsonResult> OnPostDeleteAjaxAsync(Guid uid)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return new JsonResult(new { success = false, message = "You must be logged in to delete posts." });
            }

            var result = await _blogService.DeletePostAsync(uid);
            
            if (result.Success)
            {
                return new JsonResult(new { success = true, message = "Post deleted successfully!", redirectUrl = Url.Page("/Blog/Index") });
            }

            return new JsonResult(new { success = false, message = result.Message });
        }
    }
}
