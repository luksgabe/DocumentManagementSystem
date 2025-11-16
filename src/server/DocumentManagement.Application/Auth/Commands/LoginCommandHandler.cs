using DocumentManagement.Application.Auth.DTOs;
using DocumentManagement.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentManagement.Application.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto?>
{
    private readonly IAuthRepository _repository;
    private readonly IConfiguration _config;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(IAuthRepository repository, IConfiguration config, ILogger<LoginCommandHandler> logger)
    {
        _repository = repository;
        _config = config;
        _logger = logger;
    }

    public async Task<LoginResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting login for {Email}", request.Email);

        var user = await _repository.ValidateUserAsync(request.Email, request.Password);

        if (user is null)
        {
            _logger.LogWarning("Invalid credentials for {Email}", request.Email);
            return null;
        }

        _logger.LogInformation("User {Email} authenticated successfully", request.Email);

        return new LoginResponseDto(user.Id, user.Email, user.Role, null);
    }

}