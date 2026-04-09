using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Client.Pages.Auth
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public void OnGet()
        {
            Name = User.Identity?.Name ?? "Unknown User";
            Email = User.FindFirstValue(ClaimTypes.Email) ?? "No email provided";
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "N/A";
        }
    }
}
