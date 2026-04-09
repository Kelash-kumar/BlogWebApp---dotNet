using BlogAuth.UI.Models.DTOs;
using BlogAuth.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogAuth.UI.Pages.Blog
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IBlogService _blogService;

        public EditModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [BindProperty]
        public CreatePostDto Post { get; set; } = new();

        [BindProperty]
        public Guid Uid { get; set; }

        public List<CategoryDto> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid uid)
        {
            Uid = uid;
            await LoadCategories();
            var response = await _blogService.GetPostByUidAsync(uid);
            if (!response.Success || response.Data == null)
            {
                return NotFound();
            }

            Post = new CreatePostDto
            {
                Title = response.Data.Title ?? string.Empty,
                Slug = response.Data.Slug ?? string.Empty,
                Excerpt = response.Data.Excerpt ?? string.Empty,
                Content = response.Data.Content ?? string.Empty,
                FeaturedImage = response.Data.FeaturedImage ?? string.Empty,
                Status = response.Data.Status ?? string.Empty,
                AuthorId = response.Data.Author?.Id ?? 0,
                CategoryId = response.Data.Category?.Id ?? 0,
                PublishedAt = response.Data.PublishedAt
            };

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

            var result = await _blogService.UpdatePostAsync(Uid, Post);
            if (result.Success)
            {
                return RedirectToPage("/Blog/Details", new { uid = Uid });
            }

            ModelState.AddModelError(string.Empty, result.Message);
            await LoadCategories();
            return Page();
        }
        public async Task<JsonResult> OnPostEditAjaxAsync()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Please fill all required fields correctly." });
            }

            var result = await _blogService.UpdatePostAsync(Uid, Post);
            
            if (result.Success)
            {
                return new JsonResult(new { success = true, message = "Post updated successfully!", redirectUrl = Url.Page("/Blog/Details", new { uid = Uid }) });
            }

            return new JsonResult(new { success = false, message = result.Message });
        }
    }
}
