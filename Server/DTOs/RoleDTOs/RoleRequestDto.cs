using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.RoleDTOs
{
    public class RoleRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
