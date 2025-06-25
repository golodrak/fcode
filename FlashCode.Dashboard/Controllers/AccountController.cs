using FlashCode.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace FlashCode.Dashboard.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _configuration;

        public AccountController(IHttpClientFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _factory.CreateClient();
            var apiUrl = _configuration["Api:BaseUrl"] + "/api/account/login";
            var response = await client.PostAsJsonAsync(apiUrl, model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                HttpContext.Session.SetString("jwt", result!.Token);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
    }
}
