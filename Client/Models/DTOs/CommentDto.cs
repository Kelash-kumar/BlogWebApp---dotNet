using System;
using System.Collections.Generic;

namespace BlogAuth.UI.Models.DTOs
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public int PostId { get; set; }
        public int? UserId { get; set; } // Null for guests
        public string Body { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public List<CommentResponseDto> Replies { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateCommentDto
    {
        public int PostId { get; set; }
        public int? UserId { get; set; }
        public string? GuestName { get; set; }
        public string? GuestEmail { get; set; }
        public string Body { get; set; } = string.Empty;
        public int? ParentId { get; set; }
    }

    public class UpdateCommentDto
    {
        public string Body { get; set; } = string.Empty;
    }
}
