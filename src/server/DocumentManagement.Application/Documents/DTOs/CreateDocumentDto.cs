using Microsoft.AspNetCore.Http;

namespace DocumentManagement.Application.Documents.DTOs;

public record CreateDocumentDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AccessType { get; set; }
    public IFormFile File { get; set; } = null!;
    public string[] Tags { get; set; } = Array.Empty<string>();
}
