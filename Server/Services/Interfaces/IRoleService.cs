using Server.DTOs.RoleDTOs;
using Server.Helpers;

namespace Server.Services.Interfaces
{
    public interface IRoleService
    {
        Task<PagedResult<RoleResponseDto>> GetAllRoles(
            PaginationParams pagination,
            string? search = null,
            string? sortBy = "name",
            string? sortOrder = "asc"
            );
        Task<RoleResponseDto> GetRoleById(Guid uid);
        Task<RoleResponseDto> Create(RoleRequestDto roleRequestDto);
        Task<RoleResponseDto> Update(Guid uid, RoleRequestDto roleRequestDto);
    }
}
