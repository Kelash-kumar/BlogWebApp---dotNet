using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Services;
using Client.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBlogService _blogService;

        public IndexModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public List<PostResponseDto> FeaturedPosts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var result = await _blogService.GetPostsAsync(1, 3, sortBy: "createdAt", sortDirection: "desc");
            if (result.Success && result.Data != null)
            {
                FeaturedPosts = result.Data.Data;
            }
            return Page();
        }
    }
}
