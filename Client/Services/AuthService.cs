using Client.Models;
using Client.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000/api";
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(registerDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/Auth/register", content);
                var responseBody = await response.Content.ReadAsStringAsync();
                
                var result = JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<AuthResponseDto> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDto> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
             try
            {
                var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/Auth/login", content);
                var responseBody = await response.Content.ReadAsStringAsync();
                
                var result = JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(responseBody, _jsonOptions);
                return result ?? new ApiResponse<AuthResponseDto> { Success = false, Message = "Could not parse API response." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDto> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }
    }
}
