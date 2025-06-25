using Microsoft.AspNetCore.Mvc;

namespace FlashCode.API;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public ActionResult<RegisterResponse> Register(RegisterRequest request)
    {
        var user = _service.Register(request.Token, request.FirstName, request.Company, request.Email, request.AcceptContact);
        return Ok(new RegisterResponse(user.Token, user.FirstName, user.Company, user.Email));
    }

    [HttpGet("me")]
    public ActionResult<UserDto> GetMe([FromHeader(Name = "X-Token")] string token)
    {
        var user = _service.GetByToken(token);
        if (user == null) return NotFound();
        return Ok(new UserDto(user.FirstName, user.Company, user.Email, user.AcceptContact));
    }
}

public record RegisterRequest(string? Token, string FirstName, string Company, string Email, bool AcceptContact);
public record RegisterResponse(string Token, string FirstName, string Company, string Email);
public record UserDto(string FirstName, string Company, string Email, bool AcceptContact);
