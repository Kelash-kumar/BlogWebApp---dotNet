using Client.Models;
using Client.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Client.Services
{
    public class BlogService : IBlogService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000/api";
            _jsonOptions = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<ApiResponse<PagedResult<PostResponseDto>>> GetPostsAsync(int pageNumber = 1, int pageSize = 10, string? search = null, string? sortBy = null, string sortDirection = "desc", int? authorId = null)
        {
            try
            {
                var queryParams = $"?pageNumber={pageNumber}&pageSize={pageSize}&sortDirection={sortDirection}";
                if (!string.IsNullOrEmpty(search)) queryParams += $"&search={search}";
                if (!string.IsNullOrEmpty(sortBy)) queryParams += $"&sortBy={sortBy}";
                if (authorId.HasValue) queryParams += $"&authorId={authorId.Value}";

                var response = await _httpClient.GetAsync($"{_baseUrl}/Posts{queryParams}");
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<PagedResult<PostResponseDto>>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<PagedResult<PostResponseDto>> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagedResult<PostResponseDto>> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<PostResponseDto>> GetPostByUidAsync(Guid uid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Posts/{uid}");
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<PostResponseDto>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<PostResponseDto> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PostResponseDto> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<PostResponseDto>> CreatePostAsync(CreatePostDto postDto)
        {
            try
            {
                AddAuthHeader();
                var content = new StringContent(JsonSerializer.Serialize(postDto, _jsonOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/Posts", content);
                var responseBody = await response.Content.ReadAsStringAsync();
                
                var result = JsonSerializer.Deserialize<ApiResponse<PostResponseDto>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<PostResponseDto> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PostResponseDto> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<PostResponseDto>> UpdatePostAsync(Guid uid, CreatePostDto postDto)
        {
            try
            {
                AddAuthHeader();
                var content = new StringContent(JsonSerializer.Serialize(postDto, _jsonOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/Posts/{uid}", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<PostResponseDto>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<PostResponseDto> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PostResponseDto> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<object>> DeletePostAsync(Guid uid)
        {
            try
            {
                AddAuthHeader();
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/Posts/{uid}");
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<object>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<object> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<PagedResult<CategoryDto>>> GetCategoriesAsync()
        {
            try
            {
                AddAuthHeader();
                var response = await _httpClient.GetAsync($"{_baseUrl}/Categories?pageSize=100");
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<PagedResult<CategoryDto>>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<PagedResult<CategoryDto>> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagedResult<CategoryDto>> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<List<CommentResponseDto>>> GetCommentsByPostIdAsync(int postId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Comments/post/{postId}");
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<List<CommentResponseDto>>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<List<CommentResponseDto>> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CommentResponseDto>> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<CommentResponseDto>> CreateCommentAsync(CreateCommentDto commentDto)
        {
            try
            {
                AddAuthHeader();
                var content = new StringContent(JsonSerializer.Serialize(commentDto, _jsonOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/Comments", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ApiResponse<CommentResponseDto>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<CommentResponseDto> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CommentResponseDto> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }
    }
}
