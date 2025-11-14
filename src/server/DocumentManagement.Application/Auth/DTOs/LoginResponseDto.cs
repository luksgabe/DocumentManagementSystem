namespace DocumentManagement.Application.Auth.DTOs;

public record LoginResponseDto(string UserId, string Email, string Role, string Token);
