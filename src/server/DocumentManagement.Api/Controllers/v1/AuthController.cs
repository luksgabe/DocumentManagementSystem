using DocumentManagement.Application.Auth.Commands;
using DocumentManagement.Application.Auth.DTOs;
using DocumentManagement.Infra.Data.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentManagement.Api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;
    private readonly AppJwtSettings _appJwtSettings;

    public AuthController(IMediator mediator, IOptions<AppJwtSettings> appJwtSettings)
    {
        _mediator = mediator;
        _appJwtSettings = appJwtSettings.Value;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        LoginResponseDto? result = await _mediator.Send(command);

        if (result is null)
            return UnauthorizedResult("Invalid email or password");

        var token = GenerateJwt(result.UserId, result.Email, result.Role);

        var resultWithToken = new LoginResponseDto(result.UserId, result.Email, result.Role, token);

        return OkResult(resultWithToken);
    }

    #region Private methods

    private string GenerateJwt(string sub, string email, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appJwtSettings.SecretKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, sub),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _appJwtSettings.Issuer,
            audience: _appJwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    #endregion
}
