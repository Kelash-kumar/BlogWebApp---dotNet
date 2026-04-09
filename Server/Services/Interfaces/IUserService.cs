using Server.DTOs.UserDTOs;
using Server.Helpers;

namespace Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserResponseDto>> GetAllUsers(
            PaginationParams pagination,
            string? search = null,
            string? sortBy = "name",
            string? sortOrder = "asc"
            );
        Task<UserResponseDto?> UpdateUserAsync(Guid uid, UserRequestDto userUpdateDto);
        Task<UserResponseDto?> GetUserByIdAsync(Guid uid);
    }

}
