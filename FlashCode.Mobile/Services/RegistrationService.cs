using FlashCode.Mobile.Models;
using System.Net.Http.Json;

namespace FlashCode.Mobile.Services
{
    public class RegistrationService
    {
        private readonly HttpClient _client;
        private const string RegisterUrl = "/api/account/mobile/register";
        private const string ProfileUrl = "/api/account/mobile/profile";

        public RegistrationService(HttpClient client)
        {
            _client = client;
        }

        public async Task<(bool Success, string Token, string? Error)> RegisterAsync(MobileRegisterDto dto)
        {
            var response = await _client.PostAsJsonAsync(RegisterUrl, dto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResult>();
                return (true, result!.Token, null);
            }
            return (false, string.Empty, await response.Content.ReadAsStringAsync());
        }

        public async Task<MobileProfileDto?> GetProfileAsync(string token)
        {
            var response = await _client.GetAsync($"{ProfileUrl}?token={token}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MobileProfileDto>();
            }
            return null;
        }
    }
}
