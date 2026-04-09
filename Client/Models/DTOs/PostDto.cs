namespace Client.Models.DTOs
{
    public class PostResponseDto
    {
        public int? Id { get; set; }
        public Guid? Uid { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Excerpt { get; set; }
        public string? Content { get; set; }
        public string? FeaturedImage { get; set; }
        public string? Status { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AuthorDto? Author { get; set; }
        public CategoryDto? Category { get; set; }
    }

    public class CreatePostDto
    {
        public int? AuthorId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int? CategoryId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Title { get; set; } = string.Empty;
        
        public string Slug { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        
        [System.ComponentModel.DataAnnotations.Required]
        public string Content { get; set; } = string.Empty;
        
        public string FeaturedImage { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? PublishedAt { get; set; }
    }

    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
