using BlogAuth.UI.Models;
using BlogAuth.UI.Models.DTOs;

namespace BlogAuth.UI.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    }
}
