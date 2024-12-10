using Microsoft.AspNetCore.Mvc;
using MinimalApi.Models;
using MinimalApi.Services;

namespace MinimalApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    public AuthenticationController(JwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator ??
            throw new ArgumentNullException(nameof(jwtTokenGenerator));
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<string>> Authenticate(
        AuthenticationRequestBodyDto authenticationRequestBodyDto)
    {
        var token = await _jwtTokenGenerator.GenerateToken(authenticationRequestBodyDto);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }
}

