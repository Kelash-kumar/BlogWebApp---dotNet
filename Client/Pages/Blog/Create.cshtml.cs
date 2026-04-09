using Client.Models.DTOs;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.Pages.Blog
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IBlogService _blogService;

        public CreateModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [BindProperty]
        public CreatePostDto Post { get; set; } = new();

        public List<CategoryDto> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadCategories();
            // Default status
            Post.Status = "Published";
            Post.PublishedAt = DateTime.Now;
            return Page();
        }

        private async Task LoadCategories()
        {
            var result = await _blogService.GetCategoriesAsync();
            if (result.Success && result.Data != null)
            {
                Categories = result.Data.Data;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return Page();
            }

            // Set AuthorId from claims
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                Post.AuthorId = userId;
            }

            // Simple slug generation if empty
            if (string.IsNullOrEmpty(Post.Slug))
            {
                Post.Slug = Post.Title.ToLower().Replace(" ", "-").Replace("?", "");
            }

            var result = await _blogService.CreatePostAsync(Post);
            if (result.Success)
            {
                return RedirectToPage("/Blog/Index");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            await LoadCategories();
            return Page();
        }
        public async Task<JsonResult> OnPostCreateAjaxAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return new JsonResult(new { success = false, message = "Validation Error: " + errors });
            }

            // Set AuthorId from claims
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                Post.AuthorId = userId;
            }

            // Simple slug generation if empty
            if (string.IsNullOrEmpty(Post.Slug))
            {
                Post.Slug = Post.Title.ToLower().Replace(" ", "-").Replace("?", "");
            }

            var result = await _blogService.CreatePostAsync(Post);
            
            if (result.Success)
            {
                return new JsonResult(new { success = true, message = "Post published successfully!", redirectUrl = Url.Page("/Blog/Index") });
            }

            return new JsonResult(new { success = false, message = result.Message });
        }
    }
}
