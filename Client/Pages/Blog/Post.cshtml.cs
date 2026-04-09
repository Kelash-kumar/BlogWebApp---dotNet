using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogAuth.UI.Pages.Blog
{
    public class PostModel : PageModel
    {
        public BlogPost? Post { get; set; }

        public void OnGet(int id)
        {
            // Simple mock mock logic
            var posts = new List<BlogPost>
            {
                new BlogPost { 
                    Id = 1, 
                    Title = "The Future of AI in Software Development", 
                    Category = "Technology", 
                    Excerpt = "Explore how artificial intelligence is reshaping the landscape of coding and system architecture in 2026.",
                    Author = "John Doe",
                    Date = "March 15, 2026",
                    ImageUrl = "https://images.unsplash.com/photo-1516259762381-22954d7d3ad2?q=80&w=2066&auto=format&fit=crop"
                },
                new BlogPost { 
                    Id = 2, 
                    Title = "Mastering Modern Web Aesthetics", 
                    Category = "Design", 
                    Excerpt = "Why minimalist design and glassmorphism are dominating the web design trends this year.",
                    Author = "Jane Smith",
                    Date = "March 20, 2026",
                    ImageUrl = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?q=80&w=2072&auto=format&fit=crop"
                },
                new BlogPost { 
                    Id = 3, 
                    Title = "Getting Started with .NET 10", 
                    Category = "Development", 
                    Excerpt = "A comprehensive guide to the latest features and performance improvements in the new .NET release.",
                    Author = "Alex Johnson",
                    Date = "March 25, 2026",
                    ImageUrl = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?q=80&w=2070&auto=format&fit=crop"
                }
            };

            Post = posts.FirstOrDefault(p => p.Id == id);
            
            // If post not found, provide a generic one
            if (Post == null)
            {
                Post = new BlogPost {
                    Title = "Post Not Found",
                    ImageUrl = "https://images.unsplash.com/photo-1606761568499-6d2451b23c66?q=80&w=1974&auto=format&fit=crop",
                    Content = "Sorry, the blog post you're looking for does not exist."
                };
            }
        }
    }

    public partial class BlogPost 
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
