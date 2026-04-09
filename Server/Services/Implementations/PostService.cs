using AuthDemo.DTOs.PostDtos;
using AuthDemo.Exceptions;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using AuthDemo.Services.Interfaces;
using System.Security.Claims;

namespace AuthDemo.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ISlugService _slugService;

        public PostService(IPostRepository postRepository, ISlugService slugService)
        {
            _postRepository = postRepository;
            _slugService = slugService;
        }

        public async Task<PostResponseDto> CreatePost(CreatePostDto postDto)
        {
         
            var postTitle = postDto.Title;

            // Generate new slug for post
            var existingSlug = await _postRepository.GetAllPostSlugsAsync();
            var slug = _slugService.GenerateUnique(postTitle, existingSlug);

            // Create post object
            var post = new Post
            {
                AuthorId = postDto.AuthorId,
                CategoryId = postDto.CategoryId,
                Excerpt = postDto.Excerpt,
                Slug = slug,
                Title = postTitle,
                Content = postDto.Content,
                FeaturedImage = postDto.FeaturedImage,
                PublishedAt = postDto.PublishedAt,
            };

                await _postRepository.CreatePost(post);
                var newPost = await _postRepository.GetPostByIdAsync(post.Uid);

            return MapPostResponseDtos(newPost);
        }

        public async Task<PostResponseDto> GetPostByIdAsync(Guid uid)
        {
            var post = await _postRepository.GetPostByIdAsync(uid);
            if (post == null) throw new NotFoundException("Post Not Found");

            return MapPostResponseDtos(post);
        }

        public async Task<PagedResult<PostResponseDto>> GetAllPostsAsync(
            PaginationParams pagination,
            string? search = null,
            string? sortBy = "createdAt",
            string? sortDirection = "desc"
        )
        {
            var (posts, totalRecords) = await _postRepository.GetAllPostsAsync(pagination, search, sortBy, sortDirection);
            var postDtos = posts.Select(p => MapPostResponseDtos(p)).ToList();

            return new PagedResult<PostResponseDto>
            {
                Data = postDtos,
                TotalRecords = totalRecords,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
            };
        }

        public async Task<PostResponseDto> UpdatePost(Guid uid, UpdatePostDto postDto)
        {
            var post = await _postRepository.GetPostByIdAsync(uid);
            if (post == null) throw new NotFoundException("Post Not Found");

            // Update fields
            post.Title = postDto.Title;
            post.Content = postDto.Content;
            post.Excerpt = postDto.Excerpt;
            post.CategoryId = postDto.CategoryId;
            post.FeaturedImage = postDto.FeaturedImage;
            post.UpdatedAt = DateTime.UtcNow;

            // Generate new slug if title changed (optional but common)
            if (post.Title != postDto.Title)
            {
                var existingSlugs = await _postRepository.GetAllPostSlugsAsync();
                post.Slug = _slugService.GenerateUnique(postDto.Title, existingSlugs);
            }

            await _postRepository.UpdatePost(post);
            return MapPostResponseDtos(post);
        }

        public async Task<bool> DeletePost(Guid uid)
        {
            var result = await _postRepository.DeletePost(uid);
            if (!result) throw new NotFoundException("Post Not Found");
            return result;
        }

        private static PostResponseDto MapPostResponseDtos(Post post)
        {
            return new PostResponseDto
            {
                Id = post.Id,
                Uid = post.Uid,
                AuthorId = post.AuthorId,
                CategoryId = post.CategoryId,
                Title = post.Title,
                Excerpt = post.Excerpt,
                Content = post.Content,
                Slug = post.Slug,
                FeaturedImage = post.FeaturedImage,
                Status = post.Status.ToString(),
                PublishedAt = post.PublishedAt,
                Author = new AuthorDto
                {
                    Id = post.Author.Id,
                    Name = post.Author.Name,
                    Email = post.Author.Email,
                },
                Category = new CategoryDto
                {
                    Id = post.Category.Id,
                    Name = post.Category.Name,
                    Slug = post.Category.Slug,
                },
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
            };
        }
    }
}