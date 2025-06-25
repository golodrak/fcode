using System.Net.Http.Json;

namespace FlashCode.Mobile;

public class ApiClient
{
    private readonly HttpClient _client;

    public ApiClient()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var response = await _client.PostAsJsonAsync("users/register", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
        return result ?? throw new InvalidOperationException("Invalid response");
    }

    public async Task<UserDto> GetUserAsync(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "users/me");
        request.Headers.Add("X-Token", token);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        return result ?? throw new InvalidOperationException("Invalid response");
    }
}

public class RegisterRequest
{
    public string? Token { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool AcceptContact { get; set; }
}

public class RegisterResponse
{
    public string Token { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool AcceptContact { get; set; }
}
