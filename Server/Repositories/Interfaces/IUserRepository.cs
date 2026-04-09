using Server.Helpers;
using Server.Models;

namespace Server.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid uid);

        Task CreateUserAsync(User user);

        Task<List<int>> GetValidRoleIdsAsync(List<int> roleIds);

        Task<List<Role>> GetRolesByIdsAsync(List<int> roleIds);
        Task<(List<User>, int TotalRecords)> GetAllUsers(
             PaginationParams paginationParams,
             string? search = null,
             string? sortBy = "name",
             string? sortDirection = "asc"
            );
        Task<User?> UpdateUserAsync(Guid uid, User user);

    }
}
