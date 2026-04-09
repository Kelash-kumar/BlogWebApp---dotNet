using Client.Models;
using Client.Models.DTOs;
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Client.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly IBlogService _blogService;

        public IndexModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public PagedResult<PostResponseDto> Posts { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _blogService.GetPostsAsync(PageNumber, 9, Search);
            if (response.Success && response.Data != null)
            {
                Posts = response.Data;
            }
            return Page();
        }
    }
}
