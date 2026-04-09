using Client.Models;
using Client.Models.DTOs;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Client.Pages.Blog
{
    [Authorize]
    public class MyPostsModel : PageModel
    {
        private readonly IBlogService _blogService;

        public MyPostsModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public PagedResult<PostResponseDto> Posts { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public async Task OnGetAsync()
        {
            var response = await _blogService.GetPostsAsync(PageNumber, 10, null, "createdAt", "desc", true);
            if (response.Success && response.Data != null)
            {
                Posts = response.Data;
            }
        }
    }
}
