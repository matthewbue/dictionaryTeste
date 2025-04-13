using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
    {
        try
        {
            await _authService.SignUpAsync(signUpDto);
            return Ok(new { message = "Usuário registrado com sucesso." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
    {
        var token = await _authService.SignInAsync(signInDto);
        if (token == null)
            return Unauthorized();

        return Ok(new { Token = token });
    }
}
