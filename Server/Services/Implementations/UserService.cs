using Server.DTOs.UserDTOs;
using Server.Helpers;
using Server.Models;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<PagedResult<UserResponseDto>> GetAllUsers(
            PaginationParams pagination,
            string? search = null,
            string? sortBy = "name",
            string? sortDirection = "asc")
        {
            if (pagination.PageNumber < 1) pagination.PageNumber = 1;
            if (pagination.PageSize < 1) pagination.PageSize = 10;

            var (users, totalRecords) = await _userRepository
                .GetAllUsers(pagination, search, sortBy, sortDirection);

            users ??= new List<User>();

            var userDtos = users.Select(u => MapToUserResponseDto(u)!).ToList();

            return new PagedResult<UserResponseDto>
            {
                Data = userDtos,
                TotalRecords = totalRecords,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<UserResponseDto?> UpdateUserAsync(Guid uid, UserRequestDto userUpdateDto)
        {
            var updatedUser = await _userRepository.UpdateUserAsync(uid, new User
            {
                Name = userUpdateDto.Name ?? string.Empty,
                Bio = userUpdateDto.Bio ?? string.Empty,
                Avatar = userUpdateDto.ProfilePictureUrl ?? string.Empty
            });

            return MapToUserResponseDto(updatedUser);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid uid)
        {
            var user = await _userRepository.GetUserByIdAsync(uid);

            return MapToUserResponseDto(user);
        }
        private static UserResponseDto? MapToUserResponseDto(User? user)
        {
            if (user == null) return null;
            return new()
        {
            Id = user.Id,
            Uuid = user.Uid.ToString(),
            Name = user.Name,
            Email = user.Email,
            Bio = user.Bio,
            AvatarUrl = user.Avatar,
            Status = user.Status,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Roles = user.UserRoles?.Select(ur => ur.Role!.Name).ToList()
        };
        }
    }
}
