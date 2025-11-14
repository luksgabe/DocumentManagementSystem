namespace DocumentManagement.Application.Auth.DTOs;
public record LoginRequestDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
