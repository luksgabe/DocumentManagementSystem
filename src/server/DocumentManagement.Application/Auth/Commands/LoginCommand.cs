using DocumentManagement.Application.Auth.DTOs;
using MediatR;

namespace DocumentManagement.Application.Auth.Commands;
public sealed record LoginCommand(string Email, string Password)
    : IRequest<LoginResponseDto>;