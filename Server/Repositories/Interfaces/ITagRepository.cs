using Server.Helpers;
using Server.Models;

namespace Server.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<(List<Tag>, int totalRecords)> GetAllTagsAsync(
             PaginationParams paginationParams,
             string? search = null,
             string? sortBy = null,
             string? sortDirection = "asc"
            );
        Task<Tag> GetTagByIdAsync(Guid uid);
        Task<Tag> GetTagBySlugAsync(string slug);
        Task<Tag> CreateTagAsync(Tag tag);
        Task<Tag> UpdateTagAsync(Guid uid, Tag Tag);
        Task<bool> DeleteTagAsync(Guid uid);
        Task SaveChangesAsync();
        Task<List<string>> GetAllTagSlugAsync();

    }
}
